# My_CustomBehaviorTree

This is my WIP custom implementation of a behavior tree for unity. It is inspired by Chris Simpson's article (https://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php) and
EngiGames tutorial (https://www.youtube.com/watch?v=MQEnrwAswr8).

It aims to allow to quickly setup an AI's behavior without a line of code and to easily code new behavior if needed.

## INSTALLATION

In order to use this repo, you will need to import Xnode (https://assetstore.unity.com/packages/tools/visual-scripting/xnode-104276) in your unity project.

## USAGE

How it works
in order to fully understand I recommend that you have some prior knowledges about Behavior trees (if not the gamasutra article is a good starting point)
and about Xnode as well (https://github.com/Siccity/xNode) since this repo heavily rely on Xnode's graphs implementation.
This repository is thought as submodule, this means that it is not a complete Unity project.

### BEGIN
in /Test you will have a test scene which include two agents one that wander in an area and another that will run away from the wandering agent.
This folder include everything that you might need to understand this asset.

### CODE
In order to add new node to this basic behavior tree implementation you have to create "node C# script" and make it inherit from BT.
You may also want to put it on a specific namespace in order to sort them according to their type (such namespace include : "BT.Composite", "BT.Decorator", "BT.Leaves" or "BT.Debug").
By doing so your node WILL have to implement the Run method that have to return a BTState.
