using System.Runtime.InteropServices;
using TaffySharp;
using TaffySharp.Lib;

namespace TaffySharp.Tests
{
    [TestClass]
    public unsafe sealed class TaffyTreeTests
    {
        [TestMethod]
        public void TestRelativeLayout()
        {
            using var tree = new TaffyTree();

            var child = tree.NewLeaf(new Style()
            {
                Size = new(50f, 50f),
            });

            var root = tree.NewWithChildren(new Style()
            {
                Size = new(100f, 100f),
            }, child);

            var didComputeLayout = root.ComputeLayout(new(100f, 100f));
            Assert.IsTrue(didComputeLayout);

            var didGetChildLayout = child.GetLayout(out var childLayout);
            Assert.IsTrue(didGetChildLayout);
            Assert.AreEqual(childLayout.Location.X, 0f);
            Assert.AreEqual(childLayout.Location.Y, 0f);
            Assert.AreEqual(childLayout.Size.Width, 50f);
            Assert.AreEqual(childLayout.Size.Height, 50f);
        }

        [TestMethod]
        public void TestAbsoluteLayout()
        {
            using var tree = new TaffyTree();

            var child = tree.NewLeaf(new Style()
            {
                Position = Position.Absolute,
                Size = new(Dimension.FromLength(50f), Dimension.FromLength(50f)),
            });

            var root = tree.NewWithChildren(new Style()
            {
                Position = Position.Relative,
                Size = new(Dimension.FromLength(100f), Dimension.FromLength(100f)),
            }, child);

            var didComputeLayout = root.ComputeLayout(new(100f, 100f));
            Assert.IsTrue(didComputeLayout);

            var didGetChildLayout = child.GetLayout(out var childLayout);
            Assert.IsTrue(didGetChildLayout);
            Assert.AreEqual(0f, childLayout.Location.X);
            Assert.AreEqual(0f, childLayout.Location.Y);
            Assert.AreEqual(50f, childLayout.Size.Width);
            Assert.AreEqual(50f, childLayout.Size.Height);
        }

        [TestMethod]
        public void TestMargin()
        {
            using var tree = new TaffyTree();

            var child = tree.NewLeaf(new Style()
            {
                Margin = new(10f),
                Size = new(50f, 50f),
            });

            var root = tree.NewWithChildren(new Style()
            {
                Size = new(100f, 100f),
            }, child);

            var didComputeLayout = root.ComputeLayout(new(100f, 100f));
            Assert.IsTrue(didComputeLayout);

            var didGetChildLayout = child.GetLayout(out var childLayout);
            Assert.IsTrue(didGetChildLayout);
            Assert.AreEqual(10f, childLayout.Location.X);
            Assert.AreEqual(10f, childLayout.Location.Y);
            Assert.AreEqual(50f, childLayout.Size.Width);
            Assert.AreEqual(50f, childLayout.Size.Height);
        }

        [TestMethod]
        public void TestPadding()
        {
            using var tree = new TaffyTree();

            var child = tree.NewLeaf(new Style()
            {
                Padding = new(LengthPercentageAuto.FromLength(10f)),
                Size = new(Dimension.FromLength(50f), Dimension.FromLength(50f)),
                BoxSizing = BoxSizing.ContentBox
            });

            var root = tree.NewWithChildren(new Style()
            {
                Size = new(Dimension.FromLength(100f), Dimension.FromLength(100f)),
            }, child);

            var didComputeLayout = root.ComputeLayout(new(100f, 100f));
            Assert.IsTrue(didComputeLayout);

            var didGetChildLayout = child.GetLayout(out var childLayout);
            Assert.IsTrue(didGetChildLayout);
            Assert.AreEqual(0f, childLayout.Location.X);
            Assert.AreEqual(0f, childLayout.Location.Y);
            Assert.AreEqual(70f, childLayout.Size.Width);
            Assert.AreEqual(70f, childLayout.Size.Height);
        }

        [TestMethod]
        public void TestPercentages()
        {
            using var tree = new TaffyTree();

            var child = tree.NewLeaf(new Style()
            {
                Size = new(Dimension.FromPercentage(0.5f), Dimension.FromPercentage(1f)),
            });

            var root = tree.NewWithChildren(new Style()
            {
                Size = new(Dimension.FromPercentage(1f), Dimension.FromPercentage(1f)),
            }, child);

            var didComputeLayout = root.ComputeLayout(new(100f, 100f));
            Assert.IsTrue(didComputeLayout);

            var didGetChildLayout = child.GetLayout(out var childLayout);
            Assert.IsTrue(didGetChildLayout);
            Assert.AreEqual(0f, childLayout.Location.X);
            Assert.AreEqual(0f, childLayout.Location.Y);
            Assert.AreEqual(50f, childLayout.Size.Width);
            Assert.AreEqual(100f, childLayout.Size.Height);
        }
    }
}
