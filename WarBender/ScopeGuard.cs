using System;
using System.Diagnostics;

namespace WarBender {
    internal struct ScopeGuard : IDisposable {
        private Action _actions;

        public void Add(Action action) {
            _actions += action;
        }

        public void Dispose() {
            try {
                _actions?.Invoke();
            } catch (Exception ex) {
                Trace.WriteLine(ex, nameof(ScopeGuard));
            }
        }

        public void Disarm() {
            _actions = null;
        }
    }
}
