using System.Runtime.InteropServices;
using TaffySharp;
using TaffySharp.Lib;

namespace TaffySharp.Tests
{
    [TestClass]
    public unsafe sealed class Test1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                using var tree = new TaffyTree();

                var style = new Style()
                    .SetSize(new()
                    {
                        width = new() { dim = (int)Dimension.Length, value = 100 },
                        height = new() { dim = (int)Dimension.Percent, value = 0.25f }
                    });

                var child = tree.CreateNode(style);

                var containerStyle = new Style()
                    .SetSize(new()
                    {
                        width = new() { dim = (int)Dimension.Length, value = 200 },
                        height = new() { dim = (int)Dimension.Length, value = 200 }
                    });

                var container = tree.NewWithChildren(containerStyle, child);

                var didComputeLayout = tree.ComputeLayout(container, new()
                {
                    width = new() { dim = (int)Dimension.Length, value = 200 },
                    height = new() { dim = (int)Dimension.Length, value = 200 }
                });

                Assert.IsTrue(didComputeLayout);

                var didGetLayout = tree.GetLayout(child, out var layout);

                Assert.IsTrue(didGetLayout);

                Assert.AreEqual(100f, (layout.size[0]));
                Assert.AreEqual(50f, layout.size[1]);

                Assert.AreEqual(0f, layout.location[0]);
                Assert.AreEqual(0f, layout.location[1]);

                tree.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
