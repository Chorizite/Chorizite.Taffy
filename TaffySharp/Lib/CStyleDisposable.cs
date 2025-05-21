using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TaffySharp.Lib
{
    internal class CStyleDisposable : IDisposable
    {
        public IntPtr NativePtr { get; }
        private List<IntPtr> _allocatedPointers;

        public CStyleDisposable(IntPtr cStylePtr, List<IntPtr> allocatedPointers)
        {
            NativePtr = cStylePtr;
            _allocatedPointers = allocatedPointers;
        }

        public void Dispose()
        {
            foreach (var ptr in _allocatedPointers)
            {
                Marshal.FreeHGlobal(ptr);
            }
            _allocatedPointers.Clear();
        }
    }
}
