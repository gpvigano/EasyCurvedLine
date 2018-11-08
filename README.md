# Easy Curved Line Renderer
### Easy rendering and editing of curved lines in Unity
**Requires Unity 2017.1 or higher.**

This is a project born from the [Easy Curved Line Renderer utility](https://forum.unity.com/threads/easy-curved-line-renderer-free-utility.391219/#post-3654403) originally created by [AcrylicCode](https://github.com/AcrylicCode) and posted on the Work In Progress forum of Unity, then modified by [GPV](https://github.com/gpvigano).

[AcrylicCode](https://github.com/AcrylicCode) discovered how to easily smooth out any set of positions, thus easing the creation of some simple Curved Line Renderers.

[GPV](https://github.com/gpvigano) adapted the original project to the newer versions of Unity, making some changes and exploiting the line texturing feature added since Unity 2017.1.

**Also useful for rendering solid lines (e.g. wires, pipes, speech bubble tails...)**

 ![image](https://forum.unity.com/attachments/unity-2018-02-20-11-16-25-81-gif.267478/)

*Animation in the included example scene*

## Features
Included:
* scripts:
  * LineSmoother
    * main utility script for smoothing out positions
    * static function so it can be accessed without being attached to a gameobject
    * feed it an array of Vector3 and a segment size and you will get back a smoothed out array!
    * segment size is world space and so the number of line segments in between points adjusts based on distance between points
  * CurvedLineRenderer
    * example use case for smoothed out positions
    * add new points and it will automatically update to include the new position!
    * example scene is provided with a regular curved line and a curved line with physics applied to each point
  * CurvedLinePoint
    * draws gizmo and sends back to CurvedLineRenderer to update line when the point is moved
* textures and materials:
  * normal maps to make curved lines look like their were solid
  * sample ready-to-use materials for lines (unlit and wires or pipes)

 ![image](https://forum.unity.com/attachments/curvedline001-png.177295/)

*Curved lines and their gizmos*


## Documentation

An example scene is provided to demonstrate different usages of the included assets.

To see how Easy Curved Line Renderer work,
see `EasyCurvedLine` example scene in `Assets/EasyCurvedLine/Examples`.

### Acknowledgements:

The first implementation comes from the [Easy Curved Line Renderer] post on Unity forum.
Some new features were inspired by people who suggested or asked for them, and all the people who shared their ideas on that post.
Thanks in advance to all the people who contributed and will contribute in any way to this project.


### Contributing

Contributions from you are welcome!

If you find bugs or you have any new idea for improvements and new features you can raise an issue on GitHub (please follow the suggested template, filling the proper sections). To open issues or make pull requests please follow the instructions in [CONTRIBUTING.md](https://github.com/gpvigano/EasyCurvedLine/blob/master/CONTRIBUTING.md).

### License

Code released under the [MIT License](https://github.com/gpvigano/EasyCurvedLine/blob/master/LICENSE.txt).


---
To try this project with Unity go to the [repository page on GitHub](https://github.com/gpvigano/EasyCurvedLine) press the button **Clone or download** and choose [**Download ZIP**](https://github.com/gpvigano/EasyCurvedLine/archive/master.zip). Save and unzip the archive to your hard disk and then you can open it with Unity.

[Easy Curved Line Renderer]: https://forum.unity.com/threads/easy-curved-line-renderer-free-utility.391219/#post-3654403

