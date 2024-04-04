namespace Node
{
    //Te falta poner los delegados
    public delegate void VisitDelegate(T node);
    public delegate bool CheckDelegate<T>(T element);
    public class Node<T>
    { 
        private T _item;
        private List<Node<T>> _children = new List<Node<T>>();
        private Node<T> _parent;

        public Node()
        {

        }
        public Node(T item)
        {
            if (item != null)
                _item = item;
        }
        public Node(T item, Node<T> parent)
        {
            _item = item;
            SetParent(parent);
        }

        public Node<T> Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                SetParent(Parent);
            }
        }
        public bool IsRoot => _parent == null;

        public bool IsLeaf => this._children.Count == 0 && this._parent != null;

        public int ChildCount => _children.Count;

        public int Level => GetLevel();

        public Node<T> Root => GetRoot();

        public int GetLevel()
        {
            if (IsRoot)
                return 0;

            return _parent.GetLevel() + 1;
        }

        public Node<T> GetRoot() //=>Parent == null ? this : _parent.GetRoot();
        {
            if (Parent == null)
                return this;

            return _parent.GetRoot();
        }

        public Node<T> GetChildrenAt(int index)
        {
            if (index <= _children.Count)
                return _children[index];
            return null;
        }

        public void UnLink()
        {
            if (_parent == null)
                return;

            _parent._children.Remove(this);
            _parent = null;
        }
        public int IndexOf(Node<T> node)
        {
            if (node == null || _children.Count == 0)
                return -1;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] == node)
                    return i;
            }
            return -1;
        }
        public void Remove(Node<T> child)
        {
            if (child == null || _children.Count == -1)
                return;
            int index = IndexOf(child);
            if (index >= 0)
            {
                _children[index].UnLink();
            }

        }
        public void AddChild(Node<T> node)
        {
            if (node == null || node._parent == this || this == node)
                return;
            if (ContainsAncestor(node))
                return;
            node.UnLink();
            _children.Add(node);
            node._parent = this;
        }



        public void SetParent(Node<T> newParent)
        {
            if (newParent == null)
                UnLink();
            else
                newParent.AddChild(this);
        }
        public bool HasSibling(Node<T> node)
        {
            if (node == null)
                return false;
            return _parent == node._parent;
        }
        public bool ContainsAncestor(Node<T> node)
        {
            if (node == null || _parent == null)
                return false;
            if (node == _parent)
                return true;

            return _parent.ContainsAncestor(node);
        }
        public bool ContainsDescendant(Node<T> node)
        {
            if (node == null)
                return false;
            if (node == _parent)
                return false;
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i] == node)
                    return true;

                else
                    _children[i].ContainsDescendant(node);
            }
            return false;
        }
        public void Visit(VisitDelegate<T> visitor) //
        {
            if (visitor == null)
                return;
            visitor(this);
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Visit(visitor);
            }

        }
        public Node<T> FindNode(CheckDelegate<T> checker) //
        {
            if (checker == null)
                return null;
            if (checker(_item))
                return this;
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                var node = child.FindNode(checker);
                if (node != null)
                    return node;
            }
            return null;

        }
        public void FindNode1(CheckDelegate<T> checker, List<Node<T>> nodes)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                var child = _children[i];
                child.FindNode1(checker, nodes);
                if (child != null)
                    nodes.Add(child);
            }
        }

        public List<Node<T>> FindNodeList(CheckDelegate<T> checker)
        {
            List<Node<T>> node = new List<Node<T>>();
            if (checker == null)
                return new List<Node<T>>();
            if (checker(this._item))
                node.Add(this);
            FindNode1(checker, node);

            return node;


        }

        /*
        public List<Node<T>> FindNodeList(CheckDelegate<T> checker) //
        {
            List<Node<T>> nodes = new List<Node<T>>();
            if(FindNode(checker)!= null)
            {
                nodes.Add(FindNode(checker));
            }
            return nodes;
        }*/
        public List<Node<T>> Filter(CheckDelegate<T> checker) //
        {
            if (checker == null)
                return null;
            return FindNodeList(checker);
        }
    }
}
