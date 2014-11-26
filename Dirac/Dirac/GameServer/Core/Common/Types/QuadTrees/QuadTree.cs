using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows;

using Dirac.GameServer.Core;

namespace Dirac.GameServer.Types
{
    public class QuadTree
    {
        /// <summary>
        /// Locker for all operations
        /// </summary>
        public object quadtreeLocker = new object();

        /// <summary>
        /// The smallest size a leaf will split into.
        /// </summary>
        public Size MinimumLeafSize { get; set; }

        /// <summary>
        /// Maximum number of objects per left before it's forced to split into sub-quadrans.
        /// </summary>
        public int MaximumObjectsPerLeaf { get; set; }

        /// <summary>
        /// The root QuadNode.
        /// </summary>
        public QuadNode RootNode { get; set; }

        /// <summary>
        /// Lookup dictionary mapping object's to nodes.
        /// </summary>
        private readonly ConcurrentDictionary<WorldObject, QuadNode> _objectToNodeLookup =
            new ConcurrentDictionary<WorldObject, QuadNode>();


        private ConcurrentQueue<Action> actionQueues = new ConcurrentQueue<Action>();

        /// <summary>
        /// Creates a new QuadTree.
        /// </summary>
        /// <param name="minimumLeafSize">The smallest size a leaf will split into.</param>
        /// <param name="maximumObjectsPerLeaf">Maximum number of objects per left before it's forced to split into sub-quadrans.</param>
        public QuadTree(Size minimumLeafSize, int maximumObjectsPerLeaf)
        {
            this.RootNode = null;
            this.MinimumLeafSize = minimumLeafSize;
            this.MaximumObjectsPerLeaf = maximumObjectsPerLeaf;
        }

        /// <summary>
        /// Inserts a new object.
        /// </summary>
        /// <param name="object">The object to be inserted.</param>
        public void Insert(WorldObject @object)
        {
            //this.actionQueues.Enqueue(() =>
                //{
            lock (quadtreeLocker)
            {
                if (RootNode == null) // create a new root-node if it does not exist yet.
                {
                    var rootSize = new Size(System.Math.Ceiling(@object.Bounds.Width / MinimumLeafSize.Width),
                                            System.Math.Ceiling(@object.Bounds.Height / MinimumLeafSize.Height));

                    double multiplier = System.Math.Max(rootSize.Width, rootSize.Height);
                    rootSize = new Size(MinimumLeafSize.Width * multiplier, MinimumLeafSize.Height * multiplier);
                    var center = new Point(@object.Bounds.X + @object.Bounds.Width / 2,
                                           @object.Bounds.Y + @object.Bounds.Height / 2);
                    var rootOrigin = new Point(center.X - rootSize.Width / 2, center.Y - rootSize.Height / 2);

                    this.RootNode = new QuadNode(new Rect(rootOrigin, rootSize));
                }

                while (!RootNode.Bounds.Contains(@object.Bounds))
                // if root-node's bounds does not contain object, expand the root.
                {
                    this.ExpandRoot(@object.Bounds);
                }

                this.InsertNodeObject(RootNode, @object); // insert the object to rootNode.
            }
                //});
        }

        /// <summary>
        /// Removes object from it's node.
        /// </summary>
        /// <param name="object">The object to remove.</param>
        public void Remove(WorldObject @object)
        {
            lock (quadtreeLocker)
            {
                try
                {

                    if (_objectToNodeLookup.ContainsKey(@object))
                    {
                        QuadNode containingNode = _objectToNodeLookup[@object]; // get the object.

                        RemoveObjectFromNode(@object); // remove it.

                        if (containingNode.Parent != null)
                            checkChildNodes(containingNode.Parent); // check all child nodes of parent.
                    }
                    else //!_objectToNodeLookup.ContainsKey(@object)
                    {
                        Logging.LogManager.DefaultLogger.Warn("QuadTree:Remove() - Object not found in dictionary for removal. DynamicID [{0}]", @object.DynamicID);
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogManager.DefaultLogger.Error(ex.Message);
                }
            }
        }

        public void Update()
        {
            Action toExecute;
            while (this.actionQueues.Count != 0)
            {
                if (this.actionQueues.Count > 10)
                    Logging.LogManager.DefaultLogger.Warn("this.actionQueues.Count > 10");
                if (this.actionQueues.TryDequeue(out toExecute))
                {
                    toExecute.Invoke();
                }
                else
                {
                    Logging.LogManager.DefaultLogger.Error("[QuadTree] this.actionQueues.TryDequeue(out toExecute)");
                }
            }
        }

        /// <summary>
        /// Returns list of objects with type T in given bounds.
        /// </summary>
        /// <typeparam name="T">Type of object to query for.</typeparam>
        /// <param name="bounds">The bounds for query.</param>
        /// <returns>Returns list of objects.</returns>
        public List<T> Query<T>(Rect bounds) where T : WorldObject
        {
            var results = new List<T>();
            if (this.RootNode != null)
                this.Query(bounds, RootNode, results);
            return results;
        }

        public List<T> Query<T>(Circle proximity) where T : WorldObject
        {
            var results = new List<T>();
            if (this.RootNode != null)
                this.Query(proximity, RootNode, results);
            return results;
        }

        /// <summary>
        /// Queries given bounds for node for object type T.
        /// </summary>
        /// <typeparam name="T">The objects to look for.</typeparam>
        /// <param name="bounds">The bounds.</param>
        /// <param name="node">The node to queryy.</param>
        /// <param name="results">The objects found.</param>
        private void Query<T>(Rect bounds, QuadNode node, List<T> results) where T : WorldObject
        {
            lock (quadtreeLocker)
            {
                if (node == null) return;
                if (!bounds.IntersectsWith(node.Bounds))
                    return; // if given bounds does not intersect with given node return.

                foreach (var @object in node.ContainedObjects.Values) // loop through objects contained in node.
                {
                    if (!(@object is T)) continue; // if object is not in given type, skip it.

                    if (bounds.IntersectsWith(@object.Bounds))
                        // if object's bounds intersects our given bounds, add it to results list.
                        results.Add(@object as T);
                }

                foreach (QuadNode childNode in node.Nodes) // query child-nodes too.
                {
                    this.Query(bounds, childNode, results);
                }
            }
        }

        private void Query<T>(Circle proximity, QuadNode node, List<T> results) where T : WorldObject
        {
            lock (quadtreeLocker)
            {
                if (node == null) return;
                if (!proximity.Intersects(node.Bounds))
                    return; // if given proximity circle does not intersect with given node return.

                foreach (var @object in node.ContainedObjects.Values) // loop through objects contained in node.
                {
                    if (!(@object is T)) continue; // if object is not in given type, skip it.

                    if (proximity.Intersects(@object.Bounds))
                        // if object's bounds intersects our given proximity circle, add it to results list.
                        results.Add(@object as T);
                }

                foreach (QuadNode childNode in node.Nodes) // query child-nodes too.
                {
                    this.Query(proximity, childNode, results);
                }
            }
        }

        /// <summary>
        /// Expands the root node bounds.
        /// </summary>
        /// <param name="newChildBounds"></param>
        private void ExpandRoot(Rect newChildBounds)
        {
            bool isNorth = RootNode.Bounds.Y < newChildBounds.Y;
            bool isWest = RootNode.Bounds.X < newChildBounds.X;

            Direction rootDirection = isNorth // find the direction.
                                          ? (isWest ? Direction.NorthWest : Direction.NorthEast)
                                          : (isWest ? Direction.SouthWest : Direction.SouthEast);

            double newX = (rootDirection == Direction.NorthWest || rootDirection == Direction.SouthWest)
                              ? RootNode.Bounds.X
                              : RootNode.Bounds.X - RootNode.Bounds.Width;
            double newY = (rootDirection == Direction.NorthWest || rootDirection == Direction.NorthEast)
                              ? RootNode.Bounds.Y
                              : RootNode.Bounds.Y - RootNode.Bounds.Height;

            var newRootBounds = new Rect(newX, newY, RootNode.Bounds.Width * 2, RootNode.Bounds.Height * 2);
            var newRoot = new QuadNode(newRootBounds);

            this.setupChildNodes(newRoot);
            newRoot[rootDirection] = RootNode;
            this.RootNode = newRoot;
        }

        /// <summary>
        /// Inserts object to given node or either one of it's childs.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="object">The object to be inserted.</param>
        private void InsertNodeObject(QuadNode node, WorldObject @object)
        {
            if (!node.Bounds.Contains(@object.Bounds))
                throw new Exception(
                    "QuadTree:InsertNodeObject(): This should not happen, child does not fit within node bounds");

            // If there's no child-nodes and when new object is insertedi if node's object count will be bigger then MaximumObjectsPerLeaf, force a split.
            if (!node.HasChildNodes() && node.ContainedObjects.Count + 1 > this.MaximumObjectsPerLeaf)
            {
                this.setupChildNodes(node);

                var childObjects = new List<WorldObject>(node.ContainedObjects.Values); // node's child objects.
                var childrenToRelocate = new List<WorldObject>(); // child object to be relocated.

                foreach (WorldObject childObject in childObjects)
                {
                    foreach (QuadNode childNode in node.Nodes)
                    {
                        if (childNode == null)
                            continue;

                        if (childNode.Bounds.Contains(childObject.Bounds))
                        {
                            childrenToRelocate.Add(childObject);
                        }
                    }
                }

                foreach (WorldObject childObject in childrenToRelocate) // relocate the child objects we marked.
                {
                    this.RemoveObjectFromNode(childObject);
                    this.InsertNodeObject(node, childObject);
                }
            }

            // Find a child-node that the object can be inserted.
            foreach (QuadNode childNode in node.Nodes)
            {
                if (childNode == null)
                    continue;

                if (!childNode.Bounds.Contains(@object.Bounds))
                    continue;

                this.InsertNodeObject(childNode, @object);
                return;
            }

            this.addObjectToNode(node, @object); // add the object to current node.
        }

        /// <summary>
        /// Clears objects in the node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void ClearObjectsFromNode(QuadNode node)
        {
            var objects = new List<WorldObject>(node.ContainedObjects.Values);

            foreach (WorldObject quadObject in objects)
            {
                RemoveObjectFromNode(quadObject);
            }
        }

        /// <summary>
        /// Removes a given object from it's node.
        /// </summary>
        /// <param name="object">The object to remove.</param>
        private void RemoveObjectFromNode(WorldObject @object)
        {
            QuadNode node = _objectToNodeLookup[@object];
            WorldObject removedObject;
            node.ContainedObjects.TryRemove(@object.DynamicID, out removedObject);
            QuadNode removed;
            _objectToNodeLookup.TryRemove(@object, out removed);
            @object.PositionChanged -= objectPositionChanged;
        }

        /// <summary>
        /// Adds an object to a given node.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="object"></param>
        private void addObjectToNode(QuadNode node, WorldObject @object)
        {
            if (!node.ContainedObjects.TryAdd(@object.DynamicID, @object))
                throw new Exception("node.ContainedObjects.TryAdd(@object.DynamicID, @object)");

            if (!_objectToNodeLookup.TryAdd(@object, node))
                throw new Exception("node.ContainedObjects.TryAdd(@object.DynamicID, @object)");

            @object.PositionChanged += objectPositionChanged;
        }

        private void objectPositionChanged(object sender, EventArgs e)
        {
            lock (quadtreeLocker)
            {
                var @object = sender as WorldObject;

                if (@object == null)
                    return;

                if (!this._objectToNodeLookup.ContainsKey(@object))
                {
                    //Logging.LogManager.DefaultLogger.Warn("ObjectPositionChanged, !this._objectToNodeLookup.ContainsKey(@object)");
                    //esto pasa seguido... return comun y listo, no hay drama si se mueve y no existe.
                    return;
                }

                QuadNode node = this._objectToNodeLookup[@object];

                if (node.Bounds.Contains(@object.Bounds) && !node.HasChildNodes())
                    return;

                this.RemoveObjectFromNode(@object);

                this.Insert(@object);

                if (node.Parent != null)
                    this.checkChildNodes(node.Parent);
            }
        }

        /// <summary>
        /// Creates childs nodes for a given node.
        /// </summary>
        /// <param name="node">The node.</param>
        private void setupChildNodes(QuadNode node)
        {
            if (this.MinimumLeafSize.Width > node.Bounds.Width / 2 || this.MinimumLeafSize.Height > node.Bounds.Height / 2)
                // make sure we obey MinimumLeafSize.
                return;

            node[Direction.NorthWest] = new QuadNode(node.Bounds.X, node.Bounds.Y, node.Bounds.Width / 2, node.Bounds.Height / 2);
            node[Direction.NorthEast] = new QuadNode(node.Bounds.X + node.Bounds.Width / 2, node.Bounds.Y, node.Bounds.Width / 2, node.Bounds.Height / 2);
            node[Direction.SouthWest] = new QuadNode(node.Bounds.X, node.Bounds.Y + node.Bounds.Height / 2, node.Bounds.Width / 2, node.Bounds.Height / 2);
            node[Direction.SouthEast] = new QuadNode(node.Bounds.X + node.Bounds.Width / 2, node.Bounds.Y + node.Bounds.Height / 2, node.Bounds.Width / 2, node.Bounds.Height / 2);
        }

        

        /// <summary>
        /// Checks child nodes of the node.
        /// </summary>
        /// <param name="node">The parent node.</param>
        private void checkChildNodes(QuadNode node)
        {
            if (GetTotalObjectCount(node) > MaximumObjectsPerLeaf) return;

            // Move child objects into this node, and delete sub nodes
            List<WorldObject> subChildObjects = GetChildObjects(node);

            foreach (WorldObject childObject in subChildObjects)
            {
                if (node.ContainedObjects.Values.Contains(childObject)) continue;

                RemoveObjectFromNode(childObject);
                addObjectToNode(node, childObject);
            }

            if (node[Direction.NorthWest] != null)
            {
                node[Direction.NorthWest].Parent = null;
                node[Direction.NorthWest] = null;
            }

            if (node[Direction.NorthEast] != null)
            {
                node[Direction.NorthEast].Parent = null;
                node[Direction.NorthEast] = null;
            }

            if (node[Direction.SouthWest] != null)
            {
                node[Direction.SouthWest].Parent = null;
                node[Direction.SouthWest] = null;
            }

            if (node[Direction.SouthEast] != null)
            {
                node[Direction.SouthEast].Parent = null;
                node[Direction.SouthEast] = null;
            }

            if (node.Parent != null)
                checkChildNodes(node.Parent);
            else
            {
                // Its the root node, see if we're down to one quadrant, with none in local storage - if so, ditch the other three.
                int numQuadrantsWithObjects = 0;
                QuadNode nodeWithObjects = null;

                foreach (QuadNode childNode in node.Nodes)
                {
                    if (childNode == null || GetTotalObjectCount(childNode) <= 0) continue;

                    numQuadrantsWithObjects++;
                    nodeWithObjects = childNode;
                    if (numQuadrantsWithObjects > 1) break;
                }

                if (numQuadrantsWithObjects == 1)
                // if we have only one quadrand with objects, make it the new rootNode.
                {
                    foreach (QuadNode childNode in node.Nodes)
                    {
                        if (childNode != nodeWithObjects)
                            childNode.Parent = null;
                    }

                    this.RootNode = nodeWithObjects;
                }
            }
        }

        /// <summary>
        /// Returns objects for given node, including the one in it's child-node.s
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The contained object list.</returns>
        private List<WorldObject> GetChildObjects(QuadNode node)
        {
            var results = new List<WorldObject>();
            results.AddRange(node.ContainedObjects.Values);

            foreach (QuadNode childNode in node.Nodes)
            {
                if (childNode != null)
                    results.AddRange(GetChildObjects(childNode));
            }
            return results;
        }

        /// <summary>
        /// Returns total object count.
        /// </summary>
        /// <returns>The count of objects.</returns>
        public int GetTotalObjectCount()
        {
            if (RootNode == null)
                return 0;

            int count = GetTotalObjectCount(RootNode);
            return count;
        }

        /// <summary>
        /// Returns contained object count for given node - including it's child-nodes.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The count of objects.</returns>
        private int GetTotalObjectCount(QuadNode node)
        {
            int count = node.ContainedObjects.Count;
            foreach (QuadNode childNode in node.Nodes)
            {
                if (childNode != null)
                {
                    count += GetTotalObjectCount(childNode); //Hmm no me gusta. muy recursivo para grandes numeros
                }
            }
            return count;
        }

        /// <summary>
        /// Returns total node count.
        /// </summary>
        /// <returns>The count of nodes.</returns>
        public int GetQuadNodeCount()
        {
            if (RootNode == null)
                return 0;

            int count = GetQuadNodeCount(RootNode, 1);
            return count;
        }

        /// <summary>
        /// Returns node count for given node including it's childs.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="count">Starting value for count.</param>
        /// <returns>The counf on nodes.</returns>
        private int GetQuadNodeCount(QuadNode node, int count)
        {
            if (node == null) return count;

            foreach (QuadNode childNode in node.Nodes)
            {
                if (childNode != null)
                    count++;
            }
            return count;
        }

        /// <summary>
        /// Returns list of all nodes.
        /// </summary>
        /// <returns>List of all nodes.</returns>
        public List<QuadNode> GetAllNodes()
        {
            var results = new List<QuadNode>();

            if (RootNode != null)
            {
                results.Add(RootNode);
                GetChildNodes(RootNode, results);
            }

            return results;
        }

        /// <summary>
        /// Adds all child-nodes to given results collection.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="results">The list to add.</param>
        private void GetChildNodes(QuadNode node, ICollection<QuadNode> results)
        {
            foreach (QuadNode childNode in node.Nodes)
            {
                if (childNode == null)
                    continue;

                results.Add(childNode);
                GetChildNodes(childNode, results);
            }
        }
    }

    /// <summary>
    /// Node directions.
    /// </summary>
    public enum Direction : int
    {
        NorthWest = 0,
        NorthEast = 1,
        SouthWest = 2,
        SouthEast = 3
    }
}
