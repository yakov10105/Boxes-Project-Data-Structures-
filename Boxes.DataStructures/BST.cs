using System;

namespace BinaryTree
{
    [Serializable]
    public class BST<T>
         where T : IComparable<T>
    {
        public Node root = null;

        public void Add(T value) //O(log(N))
        {
            Node tmp = root;
            if (tmp == null)
            {
                root = new Node(value);
                return;
            }
            Node newNode = new Node(value);
            while (true)
            {
                if (value.CompareTo(tmp.data) < 0)
                {
                    if (tmp.left == null)
                    {
                        tmp.left = newNode;
                        break;
                    }
                    tmp = tmp.left;
                }
                else
                {
                    if (tmp.right == null)
                    {
                        tmp.right = newNode;
                        break;
                    }
                    tmp = tmp.right;
                }
            }
        }

        public bool IsRootNull() => root == null;

        public void Remove(T data)
        {
            root = Delete(root, data);
        }
        private Node Delete(Node root, T data)
        {
            if (root == null)
                return root;
            if (data.CompareTo(root.data) < 0)
                root.left = Delete(root.left, data);
            else if (data.CompareTo(root.data) > 0)
                root.right = Delete(root.right, data);
            else
            {
                if (root.left == null)
                    return root.right;
                else if (root.right == null)
                    return root.left;
                root.data = MinValue(root.right).data;
                root.right = Delete(root.right, root.data);
            }
            return root;
        }
        private Node MinValue(Node node)
        {
            Node current = node;
            while (current.left != null)
            {
                current = current.left;
            }
            return current;
        }


        public Node Find(T value)
        {
            return this.Find(value, this.root);
        }
        private Node Find(T value, Node root)
        {
            if (root != null)
            {
                if (value.CompareTo(root.data) == 0) return root;
                if (value.CompareTo(root.data) < 0) return Find(value, root.left);
                else
                    return Find(value, root.right);
            }
            return null;
        }

        public T FindHigherValue(T wantedValue)
        {
            T retT=default;
            Node tmp = root;
            if (root == null) throw new Exception();
            while (tmp != null)
            {
                if (wantedValue.CompareTo(tmp.data) <= 0)
                {
                    retT = tmp.data;
                    tmp = tmp.left;
                }
                else
                    tmp = tmp.right;
            }
            if (retT.CompareTo(wantedValue) > 0)
                return retT;
            else
                throw new Exception();
        }

        public int GetDepth()
        {
            return DepthSearch(root);
        }
        private int DepthSearch(Node nd)
        {
            if (nd == null) return 0;

            // return Math.Max(DepthSearch(nd.left), DepthSearch(nd.right));

            int lftSubTree = DepthSearch(nd.left);
            int rgtSubTree = DepthSearch(nd.right);
            return Math.Max(lftSubTree, rgtSubTree) + 1;
        }
        public void ActInOrder(Action<T> singleItemAction)//inOrder
        {
            Act(root, singleItemAction);
        }
        private void Act(Node currentNode, Action<T> singleItemAction)
        {
            if (currentNode == null) return;

            Act(currentNode.left, singleItemAction);
            singleItemAction(currentNode.data);
            Act(currentNode.right, singleItemAction);
        }

        public override string ToString()
        {
            return $"{root.data} Tree";
        }

        [Serializable]
        public class Node
        {
            public Node left;
            public Node right;
            public T data;

            public Node(T data)
            {
                this.data = data;
            }
            public override string ToString()
            {
                return $"{data} Node";
            }
        }
    }

}
