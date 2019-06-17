# SandTest
A sand 'physics' demo in Unity, initially put together in an afternoon.

<img src = "http://www.synert.co.uk/images/code/sand.png"/>

<a href = "https://www.youtube.com/watch?v=lxoE0yVbmjI">Demo video</a>

#### WHAT IT DO?
tl;dr *it's all a big hack where i just smooth the mesh*

The underlying idea is pretty simple- it creates a mesh of a given size, randomizes the heights, and then sand 'flows' from node to node based on the height difference. As of right now, none of it's physically accurate, which gets pretty obvious when you start introducing very large values (as seen in the default scene). There's a few parameters to tweak the minimum difference for flow to occur, the maximum at which the fastest rate is achieved, and then the maximum rate of flow.

Check out the default scene for how to set it up properly on an object, all the relevant code is contained in 'SandMesh.cs'.

#### CAN I USE THIS?
Sure, feel free to expand on this or add things. It has a disabled collider right now which can be updated with the rendered mesh, no idea how it'd work out in any kind of game though. 

There's a few things that I'm planning to add later down the line:
- try to keep the volume more consistent
- better support for objects, there's a spot in the middle that acts as a blocker to test the concept
- tools to add/remove sand
- make objects displace sand based on mass and speed
- wind

#### KNOWN BUGS
- bigger grid sizes start to break, the largest I've had working is 250x250. Not sure what the actual upper limit is, or why it breaks yet.
- using bigger max transfer rates with a smaller max difference will cause a lot of weird issues- you can see this starting to occur at the end of the demo video.
- volume is not currently preserved properly
- the UVs don't move or scale, haven't figured that one out yet
- the min/max settings are not affected by the scale, causing it to look strange
- often ends up with very angular shapes, doesn't really create mounds properly

#### ASSETS USED
Sand texture/normal was sourced from <a href = "http://www.texturise.club/2013/09/seamless-beach-sand-texture-bump-map.html">Texturise</a>, made by Seme Design Lab