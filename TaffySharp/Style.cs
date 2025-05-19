using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using global::TaffySharp.Lib;

namespace TaffySharp
{
        public unsafe class Style : IDisposable
        {
            private bool _disposed;
            internal c_Style* _nativeStyle;

            // Constructor for creating a new style
            public Style()
            {
                _nativeStyle = (c_Style*)Marshal.AllocHGlobal(Marshal.SizeOf<c_Style>());
                InitializeNativeStyle();
            }

            // Constructor for wrapping an existing native style pointer
            private Style(c_Style* nativeStyle)
            {
                _nativeStyle = nativeStyle;
            }

            // Initialize native style with default values
            private void InitializeNativeStyle()
            {
            var defaultStyle = new c_Style
            {
                display = 3, // Display::Block (safer default than Flex)
                box_sizing = 0,
                overflow_x = 0,
                overflow_y = 0,
                scrollbar_width = 0.0f,
                position = 0,
                inset = new c_Rect() {
                    bottom = new() { dim = 1, value = 0.0f }, // Length(0) instead of Auto
                    left = new() { dim = 1, value = 0.0f },
                    right = new() { dim = 1, value = 0.0f },
                    top = new() { dim = 1, value = 0.0f }
                },
                gap = new c_Size()
                {
                    width = new c_Length { dim = 1, value = 0.0f },
                    height = new c_Length { dim = 1, value = 0.0f }
                },
                margin = new c_Rect
                {
                    left = new c_Length { dim = 1, value = 0.0f }, // Length(0) instead of Auto
                    right = new c_Length { dim = 1, value = 0.0f },
                    top = new c_Length { dim = 1, value = 0.0f },
                    bottom = new c_Length { dim = 1, value = 0.0f }
                },
                border = new c_Rect
                {
                    left = new c_Length { dim = 1, value = 0.0f },
                    right = new c_Length { dim = 1, value = 0.0f },
                    top = new c_Length { dim = 1, value = 0.0f },
                    bottom = new c_Length { dim = 1, value = 0.0f }
                },
                padding = new c_Rect
                {
                    left = new c_Length { dim = 1, value = 0.0f },
                    right = new c_Length { dim = 1, value = 0.0f },
                    top = new c_Length { dim = 1, value = 0.0f },
                    bottom = new c_Length { dim = 1, value = 0.0f }
                },
                size = new c_Size
                {
                    width = new c_Length { dim = 0, value = 0.0f }, // Auto is usually safe
                    height = new c_Length { dim = 0, value = 0.0f }
                },
                min_size = new c_Size
                {
                    width = new c_Length { dim = 0, value = 0.0f },
                    height = new c_Length { dim = 0, value = 0.0f }
                },
                max_size = new c_Size
                {
                    width = new c_Length { dim = 0, value = 0.0f },
                    height = new c_Length { dim = 0, value = 0.0f }
                },
                flex_wrap = 0,
                flex_direction = 0,
                flex_grow = 0.0f,
                flex_shrink = 1.0f,
                flex_basis = new c_Length { dim = 1, value = 0.0f }, // Length(0) instead of Auto
                                                                     // ... other fields as before
            };
            Marshal.StructureToPtr(defaultStyle, (IntPtr)_nativeStyle, false);
            }

            // Get the native style pointer (use with caution)
            public c_Style* NativePointer => _nativeStyle;

            // Builder-like methods for setting properties
            public Style SetDisplay(int display)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->display = display;
                return this;
            }

            public Style SetBoxSizing(int boxSizing)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->box_sizing = boxSizing;
                return this;
            }

            public Style SetPosition(int position)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->position = position;
                return this;
            }

            public Style SetMargin(c_Rect margin)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->margin = margin;
                return this;
            }

            public Style SetPadding(c_Rect padding)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->padding = padding;
                return this;
            }

            public Style SetBorder(c_Rect border)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->border = border;
                return this;
            }

            public Style SetSize(c_Size size)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->size = size;
                return this;
            }

            public Style SetMinSize(c_Size minSize)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->min_size = minSize;
                return this;
            }

            public Style SetMaxSize(c_Size maxSize)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->max_size = maxSize;
                return this;
            }

            public Style SetFlexBasis(c_Length flexBasis)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->flex_basis = flexBasis;
                return this;
            }

            public Style SetFlexGrow(float flexGrow)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->flex_grow = flexGrow;
                return this;
            }

            public Style SetFlexShrink(float flexShrink)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->flex_shrink = flexShrink;
                return this;
            }

            public Style SetFlexDirection(int flexDirection)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->flex_direction = flexDirection;
                return this;
            }

            public Style SetFlexWrap(int flexWrap)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->flex_wrap = flexWrap;
                return this;
            }

            public Style SetGap(c_Size gap)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->gap = gap;
                return this;
            }

            public Style SetAlignItems(int alignItems, bool hasAlignItems = true)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->align_items = alignItems;
                _nativeStyle->has_align_items = hasAlignItems ? 1 : 0;
                return this;
            }

            public Style SetJustifyItems(int justifyItems, bool hasJustifyItems = true)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->justify_items = justifyItems;
                _nativeStyle->has_justify_items = hasJustifyItems ? 1 : 0;
                return this;
            }

            public Style SetAlignSelf(int alignSelf, bool hasAlignSelf = true)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->align_self = alignSelf;
                _nativeStyle->has_align_self = hasAlignSelf ? 1 : 0;
                return this;
            }

            public Style SetJustifySelf(int justifySelf, bool hasJustifySelf = true)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->justify_self = justifySelf;
                _nativeStyle->has_justify_self = hasJustifySelf ? 1 : 0;
                return this;
            }

            public Style SetAlignContent(int alignContent, bool hasAlignContent = true)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->align_content = alignContent;
                _nativeStyle->has_align_content = hasAlignContent ? 1 : 0;
                return this;
            }

            public Style SetJustifyContent(int justifyContent, bool hasJustifyContent = true)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                _nativeStyle->justify_content = justifyContent;
                _nativeStyle->has_justify_content = hasJustifyContent ? 1 : 0;
                return this;
            }

            public Style SetAspectRatio(float? aspectRatio)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                if (aspectRatio.HasValue)
                {
                    _nativeStyle->aspect_ratio = aspectRatio.Value;
                    _nativeStyle->has_aspect_ratio = 1;
                }
                else
                {
                    _nativeStyle->has_aspect_ratio = 0;
                }
                return this;
            }

            // Grid-related properties (simplified, assuming external management of c_GridTrackSizing arrays)
            public Style SetGridTemplateRows(c_GridTrackSizing[] rows)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                FreeGridTemplateRows();
                if (rows != null && rows.Length > 0)
                {
                    _nativeStyle->grid_template_rows_count = (nuint)rows.Length;
                    _nativeStyle->grid_template_rows = (c_GridTrackSizing*)Marshal.AllocHGlobal(Marshal.SizeOf<c_GridTrackSizing>() * rows.Length);
                    for (int i = 0; i < rows.Length; i++)
                    {
                        _nativeStyle->grid_template_rows[i] = rows[i];
                    }
                }
                return this;
            }

            public Style SetGridTemplateColumns(c_GridTrackSizing[] columns)
            {
                if (_disposed) throw new ObjectDisposedException(nameof(Style));
                FreeGridTemplateColumns();
                if (columns != null && columns.Length > 0)
                {
                    _nativeStyle->grid_template_columns_count = (nuint)columns.Length;
                    _nativeStyle->grid_template_columns = (c_GridTrackSizing*)Marshal.AllocHGlobal(Marshal.SizeOf<c_GridTrackSizing>() * columns.Length);
                    for (int i = 0; i < columns.Length; i++)
                    {
                        _nativeStyle->grid_template_columns[i] = columns[i];
                    }
                }
                return this;
            }

            // Add similar methods for grid_auto_rows, grid_auto_columns, grid_row, grid_column, grid_auto_flow

            // Dispose pattern
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                // Free unmanaged grid arrays
                FreeGridTemplateRows();
                FreeGridTemplateColumns();
                FreeGridAutoRows();
                FreeGridAutoColumns();

                // Free the native style struct
                if (_nativeStyle != null)
                {
                    Marshal.FreeHGlobal((IntPtr)_nativeStyle);
                    _nativeStyle = null;
                }

                _disposed = true;
            }

            private void FreeGridTemplateRows()
            {
                if (_nativeStyle->grid_template_rows != null)
                {
                    Marshal.FreeHGlobal((IntPtr)_nativeStyle->grid_template_rows);
                    _nativeStyle->grid_template_rows = null;
                    _nativeStyle->grid_template_rows_count = 0;
                }
            }

            private void FreeGridTemplateColumns()
            {
                if (_nativeStyle->grid_template_columns != null)
                {
                    Marshal.FreeHGlobal((IntPtr)_nativeStyle->grid_template_columns);
                    _nativeStyle->grid_template_columns = null;
                    _nativeStyle->grid_template_columns_count = 0;
                }
            }

            private void FreeGridAutoRows()
            {
                if (_nativeStyle->grid_auto_rows != null)
                {
                    Marshal.FreeHGlobal((IntPtr)_nativeStyle->grid_auto_rows);
                    _nativeStyle->grid_auto_rows = null;
                    _nativeStyle->grid_auto_rows_count = 0;
                }
            }

            private void FreeGridAutoColumns()
            {
                if (_nativeStyle->grid_auto_columns != null)
                {
                    Marshal.FreeHGlobal((IntPtr)_nativeStyle->grid_auto_columns);
                    _nativeStyle->grid_auto_columns = null;
                    _nativeStyle->grid_auto_columns_count = 0;
                }
            }

            ~Style()
            {
                Dispose(false);
            }
        }
    }
