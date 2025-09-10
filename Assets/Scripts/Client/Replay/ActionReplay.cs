using Client.ActionEvents;
using Logic.ActionEvents;

namespace Client.Replay
{
    public class ActionReplay
    {
        public ActionEvent ActionEvent;
        public IActionEventHandler EventHandler;
        public IActionViewer Viewer;
    }
}