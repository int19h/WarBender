using System;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WarBender.UI.Design {
    internal class GameDesignerHost : ISite, IDesignerHost, IComponentChangeService {
        public IComponent Component => null;
        public IContainer Container => null;
        public bool DesignMode => true;
        public string Name { get; set; }
        public bool Loading => false;
        public IComponent RootComponent => null;
        public string RootComponentClassName => null;
        public string TransactionDescription { get; private set; }
        public bool InTransaction { get; private set; }

        public event EventHandler TransactionOpened;
        public event EventHandler TransactionOpening;
        public event DesignerTransactionCloseEventHandler TransactionClosed;
        public event DesignerTransactionCloseEventHandler TransactionClosing;
        public event ComponentChangedEventHandler ComponentChanged;
        public event ComponentChangingEventHandler ComponentChanging;

        public Game Game { get; }

        private readonly ServiceContainer _services = new ServiceContainer();

        public GameDesignerHost(Game game) {
             Game = game;

            _services.AddService(typeof(IDesignerHost), this);
            _services.AddService(typeof(IComponentChangeService), this);
            _services.AddService(typeof(TypeDescriptionProvider), GameTypeDescriptionProvider.Instance);
        }

        public object GetService(Type serviceType) => _services.GetService(serviceType);

        public DesignerTransaction CreateTransaction() => new Transaction(this);

        public DesignerTransaction CreateTransaction(string description) {
            TransactionDescription = description;
            return new Transaction(this);
        }

        protected virtual void OnTransactionOpening(EventArgs e) {
            TransactionOpening?.Invoke(this, e);
        }

        protected virtual void OnTransactionOpened(EventArgs e) {
            InTransaction = true;
            TransactionOpened?.Invoke(this, e);
        }

        protected virtual void OnTransactionClosing(DesignerTransactionCloseEventArgs e) {
            TransactionClosing?.Invoke(this, e);
        }

        protected virtual void OnTransactionClosed(DesignerTransactionCloseEventArgs e) {
            InTransaction = false;
            TransactionDescription = null;
            TransactionClosed?.Invoke(this, e);
        }

        public void OnComponentChanged(object component, MemberDescriptor member, object oldValue, object newValue) {
            ComponentChanged?.Invoke(this, new ComponentChangedEventArgs(component, member, oldValue, newValue));
        }

        public void OnComponentChanging(object component, MemberDescriptor member) {
            ComponentChanging?.Invoke(this, new ComponentChangingEventArgs(component, member));
        }

        private class Transaction : DesignerTransaction {
            private readonly GameDesignerHost _host;
            private Game.Snapshot _snapshot;

            public Transaction(GameDesignerHost host) {
                _host = host;
                host.OnTransactionOpening(EventArgs.Empty);
                host.OnTransactionOpened(EventArgs.Empty);
                host.ComponentChanging += Host_ComponentChanging;
            }

            protected override void Dispose(bool disposing) {
                base.Dispose(disposing);
                if (disposing) {
                    _host.ComponentChanging -= Host_ComponentChanging;
                    _snapshot?.Dispose();
                }
            }

            private void Host_ComponentChanging(object sender, ComponentChangingEventArgs e) {
                if (_snapshot == null && e.Component is IDataObject) {
                    _snapshot = _host.Game.CreateSnapshot();
                }
            }

            protected override void OnCommit() {
                try {
                    var e = new DesignerTransactionCloseEventArgs(true, true);
                    _host.OnTransactionClosing(e);
                    try {
                        if (_snapshot != null) {
                            _host.Game.Data.Validate();
                        }
                    } catch {
                        _snapshot.Restore();
                        throw;
                    } finally {
                        _host.OnTransactionClosed(e);
                    }
                } finally {
                    Dispose(true);
                }
            }

            protected override void OnCancel() {
                try {
                    var e = new DesignerTransactionCloseEventArgs(false, true);
                    _host.OnTransactionClosing(e);
                    _snapshot?.Restore();
                    _host.OnTransactionClosed(e);
                } finally {
                    Dispose(true);
                }
            }
        }

        #region Not implemented
        public event EventHandler Activated { add { } remove { } }
        public event EventHandler Deactivated { add { } remove { } }
        public event EventHandler LoadComplete { add { } remove { } }
        public event ComponentEventHandler ComponentAdded { add { } remove { } }
        public event ComponentEventHandler ComponentAdding { add { } remove { } }
        public event ComponentEventHandler ComponentRemoved { add { } remove { } }
        public event ComponentEventHandler ComponentRemoving { add { } remove { } }
        public event ComponentRenameEventHandler ComponentRename { add { } remove { } }

        public void Activate() => throw new NotImplementedException();
        public void AddService(Type serviceType, object serviceInstance) => throw new NotImplementedException();
        public void AddService(Type serviceType, object serviceInstance, bool promote) => throw new NotImplementedException();
        public void AddService(Type serviceType, ServiceCreatorCallback callback) => throw new NotImplementedException();
        public void AddService(Type serviceType, ServiceCreatorCallback callback, bool promote) => throw new NotImplementedException();
        public IComponent CreateComponent(Type componentClass) => throw new NotImplementedException();
        public IComponent CreateComponent(Type componentClass, string name) => throw new NotImplementedException();
        public void DestroyComponent(IComponent component) => throw new NotImplementedException();
        public IDesigner GetDesigner(IComponent component) => throw new NotImplementedException();
        public Type GetType(string typeName) => throw new NotImplementedException();
        public void RemoveService(Type serviceType) => throw new NotImplementedException();
        public void RemoveService(Type serviceType, bool promote) => throw new NotImplementedException();
        #endregion
    }
}
