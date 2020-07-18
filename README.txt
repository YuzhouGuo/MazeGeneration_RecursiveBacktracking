What algorithm have you used to create a maze?

The Random Prim's algorithm. Basic idea is that we choose a certain number of walls as the framework, and just randomly connect all the rest walls to them. If we don't have anything left in the wall array then all wall pieces should be connected. And we just have a maze.

What makes it random and dynamic?

Since we choose the starting walls by using Random number/index, every time the way Prim's algorithm connect these wall pieces is gonna be different which gives us different mazes every time we call the function as a result. But since this is a legid maze, it has one opening and one closing.

How have you tracked the path solving it? 

Sorry due to my bad time management I didn't do this part much. But the idea is that once we have the maze, we can apply some other algorithms like A* to find the discrete path (This part may be easier if we use other algorithms to generate the maze)

Why have you chosen pre-existing assets over making something yourself? Make sure to cite any sources for assets, code or otherwise, so we're sure you're not claiming you've done a 4K realistic texture of grass on your own.

For the ground texture it is hard to play around with all these colors and do it myself, so I'm just being lazy and import other people's code. But for other stuff like the floating ball and the maze itself. I've tried to make them not too ugly due to the time limit.

Any challenges you found in the assignment?

The time management. Honestly nothing is too hard but I should have started this assignemnt way way before to lower my anxiety right now.

Sorry that I didn't do some of the requirement of this assignment, but i tried to make the maze auto-generator actually working so please be generous with the points associated to that :))))`
