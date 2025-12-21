using System.Collections.Generic;

namespace Core.Arenas
{
    public class ArenaRestartHandler {
        
        private readonly List<IResettable> _resettables;

        public ArenaRestartHandler(List<IResettable> resettables) =>
            _resettables = resettables;

        public void Restart() {
            foreach (var resettable in _resettables)
                resettable.Reset();
        }
    }
}