# TaffySharp
Provides a managed csharp wrapper around [Taffy](https://github.com/DioxusLabs/taffy). Currently provides native windows dlls for x86/x64. Supports dotnet 8.0, netstandard 2.0, and framework 4.8.

## Usage:
Install nuget package `Chorizite.TaffySharp`

```
// build a new tree:
using var tree = new TaffyTree();

// create a child nodes:
var childNode = tree.NewLeaf(new Style() {
    Size = new(50f, 50f),
});

var childNode2 = tree.NewLeaf(new Style() {
    Margin = new(10f)
});

// add childNode2 as a child of childNode
childNode.AddChild(childNode2);

// add a new root container with that child node:
var container = tree.NewWithChildren(containerStyle, childNode);

if (container.ComputeLayout(new(100f, 100f))) {
    if (child.GetLayout(out var childLayout)) {
        // use layout
    }
}
```

## Building taffy_ffi (only needed when updating taffy)
- Update the taffy submodule
- Update taffy_ffi/src/lib.rs with any api changes
- Build native dlls:

```
# add targets if you don't already have them
rustup target add i686-pc-windows-msvc
rustup target add x86_64-pc-windows-msvc

# build
./build_taffy_ffi.sh

# The dlls should have been built and copied to TaffySharp/runtimes/
# The pinvoke bindings should be generated in TaffySharp/Lib/NativeMethods.g.cs
```
- Update anything needed in the managed wrappers


## TODO
- Grid support
- Calc support
- Context support for nodes
- Add targets for linux / osx
- Generate tests?
- Generate enums?