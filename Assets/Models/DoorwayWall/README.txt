Doorway wall — Unity / Blender compatible OBJ

Dimensions: X width 15, Y height 15, Z thickness 1.
Door opening: X width 5, Y height 7.5, centered horizontally, open at floor.
Bounds: X [-7.5, 7.5], Y [0, 15], Z [-0.5, 0.5].
Origin: bottom center. Y up. Model coordinate unit = one Unity unit.

Unity: drag DoorwayWall.obj from Project into the scene.
Keep model import Scale Factor at 1 and Transform scale at (1,1,1).
A neutral material is provided; assign your own Unity material if desired.
For a static wall, add a Mesh Collider with Convex OFF to preserve the opening.
Alternative: three Box Colliders (local center / size):
Left: (-5, 3.75, 0) / (5, 7.5, 1)
Right: (5, 3.75, 0) / (5, 7.5, 1)
Top: (0, 11.25, 0) / (15, 7.5, 1)

Blender: File > Import > Wavefront (.obj). Set source forward to -Z, up to Y.
This is a generated OBJ mesh, not a Blender .blend file.
Single closed manifold mesh with no internal faces, flat normals and UV coordinates.
Geometry checked: 24 vertices, 22 quads / 44 triangles; volume 187.5 cubic units.
