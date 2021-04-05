TreeLib
=======

This project produces a .Net Standard Library with Generic (LevelOrder, PreOrder, PostOrder) methods
to traverse k-nary trees in different orders of traversal.

Implementing something as complicated as a Post-Order traversal algorithm requires just:
* a project reference,
* a LINQ statement to find each set of children in the tree,
* and a simple for each loop to implement the operation on each tree node:

C# Code Sample
--------------

Console.WriteLine("(Depth First) PostOrder Tree Traversal V3");
items = TreeLib.Depthfirst.Traverse.PostOrder(root, i => i.Children);

foreach (var item in items)
{
  Console.WriteLine(item.GetPath());
}


This pattern leads to a clear-cut separation of:
* the traversal algorithm and
* the operations performed on each tree node (e.g.: `Console.WriteLine(item.GetPath());`).

The project in this repository contains a demo console project to demo its usage in more detail.

Supported Generic Traversal Methods
===================================

Breadth First
-------------

Level Order
See TreeLib.BreadthFirst.Traverse.LevelOrder implementation for:

* trees with 1 root (expects &lt;T> root as parameter)
* trees with multiple root node (expects IEnumerable&lt;T> root as parameter)

Depth First
-------------

### PreOrder
See TreeLib.BreadthFirst.Traverse.PreOrder implementation for:

* trees with 1 root (expects &lt;T> root as parameter)
* trees with multiple root node (expects IEnumerable&lt;T> root as parameter)

Postorder
---------

See TreeLib.BreadthFirst.Traverse.Postorder implementation for:

* trees with 1 root (expects &lt;T> root as parameter)
* trees with multiple root node (expects IEnumerable&lt;T> root as parameter)

Project Reference: https://github.com/Dirkster99/TreeLib/
