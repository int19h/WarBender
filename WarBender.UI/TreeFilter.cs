using System.Collections;
using System.Collections.Generic;
using BrightIdeasSoftware;

namespace WarBender.UI {
    internal class TreeFilter : IModelFilter {
        private readonly HashSet<object> _objects = new HashSet<object>();

        public TreeFilter(TreeListView treeView, IModelFilter filter) {
            Populate(treeView, treeView.Objects, filter);
        }

        private void Populate(TreeListView treeView, IEnumerable objects, IModelFilter filter) {
            if (objects == null) {
                return;
            }

            foreach (var obj in objects) {
                Populate(treeView, treeView.ChildrenGetter(obj), filter);
                if (!filter.Filter(obj)) {
                    continue;
                }

                var matched = obj;
                while (matched != null) {
                    _objects.Add(matched);
                    if (matched is IDataObjectChild child) {
                        matched = child.Parent;
                    }
                }
            }
        }

        public bool Filter(object modelObject) =>
            _objects.Contains(modelObject);
    }

}
