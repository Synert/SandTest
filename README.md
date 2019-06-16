# SandTest
A sand 'physics' demo in Unity, initially put together in an afternoon. Might expand on it?

<img src = "http://www.synert.co.uk/images/code/sand.png"/>

#### WHAT IT DO?
tl;dr *it's all a big hack where i just smooth the normals*

The underlying idea is pretty simple- it creates a mesh of a given size, randomizes the heights, and then sand 'flows' from node to node based on the height difference. As of right now, none of it's physically accurate, which gets pretty obvious when you start introducing very large values (as seen in the default scene). There's a few parameters to tweak the minimum difference for flow to occur, the maximum at which the fastest rate is achieved, and then the maximum rate of flow.

Check out the default scene for how to set it up properly on an object, all the relevant code is contained in 'SandMesh.cs'. Everything else is leftover from other experiments, I'll clean it up later.

#### CAN I USE THIS?
Sure, feel free to expand on this or add things. It has a disabled collider right now which can be updated with the rendered mesh, no idea how it'd work out in any kind of game though. 

There's a few things that I'm planning to add later down the line:
- volume is not currently preserved properly... still wouldn't be, but I can get it to look closer
- better support for objects, there's a spot in the middle that acts as a blocker to test the concept
- tools to add/remove sand
- make objects displace sand based on mass and speed