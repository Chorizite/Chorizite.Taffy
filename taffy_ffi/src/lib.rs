#![allow(non_camel_case_types)]
#![allow(non_snake_case)]

use taffy::prelude::*;
use taffy::style::Style;
use taffy::Overflow;

// MAIN

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_TaffyTree {
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_new() -> *mut TaffyTree {
    let tree = TaffyTree::new();
    Box::into_raw(Box::new(tree))
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_new_with_children(tree: *mut TaffyTree, style: *const c_Style, children: *const u64, children_len: usize) -> u64 {
    if tree.is_null() || style.is_null() || children.is_null() {
        return 0;
    }

    let tree = unsafe { &mut *tree };
    let style = unsafe { Style::from(*style) };

    // Convert raw children pointer to a slice of NodeId
    let children_slice: &[taffy::NodeId] = unsafe {
        std::slice::from_raw_parts(children as *const taffy::NodeId, children_len)
    };

    // Call new_with_children
    match tree.new_with_children(style, children_slice) {
        Ok(node) => node.into(), // Convert NodeId to u64
        Err(_) => 0,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_free(tree: *mut TaffyTree) {
    if !tree.is_null() {
        unsafe {
            drop(Box::from_raw(tree));
        }
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_enable_rounding(tree: *mut TaffyTree) {
    let tree = unsafe { &mut *tree };
    tree.enable_rounding();
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_disable_rounding(tree: *mut TaffyTree) {
    let tree = unsafe { &mut *tree };
    tree.disable_rounding();
}

// STYLE

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_Length {
    dim: i32,
    value: f32,
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_Size {
    width: c_Length,
    height: c_Length,
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_Rect {
    left: c_Length,
    right: c_Length,
    top: c_Length,
    bottom: c_Length,
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_GridIndex {
    kind: i8,
    value: i16,
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_GridPlacement {
    start: c_GridIndex,
    end: c_GridIndex,
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_GridTrackSize {
    min_size: c_Length,
    max_size: c_Length,
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_GridTrackSizing {
    repetition: i32,
    single: *const c_GridTrackSize, // Nullable pointer for single track
    repeat: *const c_GridTrackSize, // Pointer to array
    repeat_count: usize, // Length of repeat array
}

#[repr(C)]
#[derive(Copy, Clone)]
pub struct c_Style {
    // Layout mode/strategy
    display: i32,
    box_sizing: i32,
    // Overflow
    overflow_x: i32,
    overflow_y: i32,
    scrollbar_width: f32,
    // Position
    position: i32,
    inset: c_Rect,
    // Alignment
    gap: c_Size,
    // Spacing
    margin: c_Rect,
    border: c_Rect,
    padding: c_Rect,
    // Size
    size: c_Size,
    min_size: c_Size,
    max_size: c_Size,
    // Flex
    flex_wrap: i32,
    flex_direction: i32,
    flex_grow: f32,
    flex_shrink: f32,
    flex_basis: c_Length,
    // Grid container properties
    grid_template_rows: *const c_GridTrackSizing,
    grid_template_rows_count: usize,
    grid_template_columns: *const c_GridTrackSizing,
    grid_template_columns_count: usize,
    grid_auto_rows: *const c_GridTrackSize,
    grid_auto_rows_count: usize,
    grid_auto_columns: *const c_GridTrackSize,
    grid_auto_columns_count: usize,
    grid_auto_flow: i32,
    // Grid child properties
    grid_row: c_GridPlacement,
    grid_column: c_GridPlacement,
    // Size, optional
    aspect_ratio: f32,
    has_aspect_ratio: i32, // 0 for None, 1 for Some
    // Alignment, optional
    align_items: i32,
    has_align_items: i32,
    justify_items: i32,
    has_justify_items: i32,
    align_self: i32,
    has_align_self: i32,
    justify_self: i32,
    has_justify_self: i32,
    align_content: i32,
    has_align_content: i32,
    justify_content: i32,
    has_justify_content: i32,
}

// Conversion traits

trait FromIndex<T> {
    fn from_index(index: i32) -> T;
}

trait FromIndexOptional<T> {
    fn from_index(index: i32, has: i32) -> Option<T>;
}

impl FromIndex<Display> for Display {
    fn from_index(index: i32) -> Display {
        match index {
            0 => Display::None,
            1 => Display::Flex,
            2 => Display::Grid,
            3 => Display::Block,
            _ => panic!("invalid display index {}", index),
        }
    }
}

impl FromIndex<BoxSizing> for BoxSizing {
    fn from_index(index: i32) -> BoxSizing {
        match index {
            0 => BoxSizing::BorderBox,
            1 => BoxSizing::ContentBox,
            _ => panic!("invalid box_sizing index {}", index),
        }
    }
}

impl FromIndex<Overflow> for Overflow {
    fn from_index(index: i32) -> Overflow {
        match index {
            0 => Overflow::Visible,
            1 => Overflow::Hidden,
            2 => Overflow::Scroll,
            3 => Overflow::Clip,
            _ => panic!("invalid overflow index {}", index),
        }
    }
}

impl FromIndex<Position> for Position {
    fn from_index(index: i32) -> Position {
        match index {
            0 => Position::Relative,
            1 => Position::Absolute,
            _ => panic!("invalid position index {}", index),
        }
    }
}

impl FromIndex<FlexWrap> for FlexWrap {
    fn from_index(index: i32) -> FlexWrap {
        match index {
            0 => FlexWrap::NoWrap,
            1 => FlexWrap::Wrap,
            2 => FlexWrap::WrapReverse,
            _ => panic!("invalid flex_wrap index {}", index),
        }
    }
}

impl FromIndex<FlexDirection> for FlexDirection {
    fn from_index(index: i32) -> FlexDirection {
        match index {
            0 => FlexDirection::Row,
            1 => FlexDirection::Column,
            2 => FlexDirection::RowReverse,
            3 => FlexDirection::ColumnReverse,
            _ => panic!("invalid flex_direction index {}", index),
        }
    }
}

impl FromIndexOptional<AlignItems> for AlignItems {
    fn from_index(index: i32, has: i32) -> Option<AlignItems> {
        if has == 0 {
            None
        } else {
            Some(match index {
                0 => AlignItems::Start,
                1 => AlignItems::End,
                2 => AlignItems::FlexStart,
                3 => AlignItems::FlexEnd,
                4 => AlignItems::Center,
                5 => AlignItems::Baseline,
                6 => AlignItems::Stretch,
                _ => panic!("invalid align_items index {}", index),
            })
        }
    }
}

impl FromIndexOptional<AlignContent> for AlignContent {
    fn from_index(index: i32, has: i32) -> Option<AlignContent> {
        if has == 0 {
            None
        } else {
            Some(match index {
                0 => AlignContent::Start,
                1 => AlignContent::End,
                2 => AlignContent::FlexStart,
                3 => AlignContent::FlexEnd,
                4 => AlignContent::Center,
                5 => AlignContent::Stretch,
                6 => AlignContent::SpaceBetween,
                7 => AlignContent::SpaceEvenly,
                8 => AlignContent::SpaceAround,
                _ => panic!("invalid align_content index {}", index),
            })
        }
    }
}

impl FromIndex<GridAutoFlow> for GridAutoFlow {
    fn from_index(index: i32) -> GridAutoFlow {
        match index {
            0 => GridAutoFlow::Row,
            1 => GridAutoFlow::Column,
            2 => GridAutoFlow::RowDense,
            3 => GridAutoFlow::ColumnDense,
            _ => panic!("invalid grid_auto_flow index {}", index),
        }
    }
}

impl From<c_Length> for Dimension {
    fn from(length: c_Length) -> Dimension {
        match length.dim {
            0 => Dimension::auto(),
            1 => Dimension::length(length.value),
            2 => Dimension::percent(length.value),
            _ => panic!("unsupported dimension {}", length.dim),
        }
    }
}

impl From<c_Length> for LengthPercentageAuto {
    fn from(length: c_Length) -> LengthPercentageAuto {
        match length.dim {
            0 => LengthPercentageAuto::auto(),
            1 => LengthPercentageAuto::length(length.value),
            2 => LengthPercentageAuto::percent(length.value),
            _ => panic!("unsupported dimension {}", length.dim),
        }
    }
}

impl From<c_Length> for LengthPercentage {
    fn from(length: c_Length) -> LengthPercentage {
        match length.dim {
            1 => LengthPercentage::length(length.value),
            2 => LengthPercentage::percent(length.value),
            _ => panic!("unsupported dimension {}", length.dim),
        }
    }
}

impl From<c_Length> for AvailableSpace {
    fn from(length: c_Length) -> AvailableSpace {
        match length.dim {
            1 => AvailableSpace::Definite(length.value),
            3 => AvailableSpace::MinContent,
            4 => AvailableSpace::MaxContent,
            _ => panic!("unsupported dimension {}", length.dim),
        }
    }
}

impl From<c_Size> for Size<Dimension> {
    fn from(size: c_Size) -> Self {
        Size {
            width: Dimension::from(size.width),
            height: Dimension::from(size.height),
        }
    }
}

impl From<c_Size> for Size<LengthPercentage> {
    fn from(size: c_Size) -> Self {
        Size {
            width: LengthPercentage::from(size.width),
            height: LengthPercentage::from(size.height),
        }
    }
}

impl From<c_Size> for Size<AvailableSpace> {
    fn from(size: c_Size) -> Self {
        Size {
            width: AvailableSpace::from(size.width),
            height: AvailableSpace::from(size.height),
        }
    }
}

impl From<c_Rect> for Rect<LengthPercentageAuto> {
    fn from(rect: c_Rect) -> Self {
        Rect {
            left: LengthPercentageAuto::from(rect.left),
            right: LengthPercentageAuto::from(rect.right),
            top: LengthPercentageAuto::from(rect.top),
            bottom: LengthPercentageAuto::from(rect.bottom),
        }
    }
}

impl From<c_Rect> for Rect<LengthPercentage> {
    fn from(rect: c_Rect) -> Self {
        Rect {
            left: LengthPercentage::from(rect.left),
            right: LengthPercentage::from(rect.right),
            top: LengthPercentage::from(rect.top),
            bottom: LengthPercentage::from(rect.bottom),
        }
    }
}

impl From<c_Rect> for Rect<Dimension> {
    fn from(rect: c_Rect) -> Self {
        Rect {
            left: Dimension::from(rect.left),
            right: Dimension::from(rect.right),
            top: Dimension::from(rect.top),
            bottom: Dimension::from(rect.bottom),
        }
    }
}

impl From<c_GridIndex> for GridPlacement {
    fn from(grid_index: c_GridIndex) -> Self {
        match grid_index.kind {
            0 => GridPlacement::Auto,
            1 => GridPlacement::from_line_index(grid_index.value),
            2 => GridPlacement::from_span(grid_index.value as u16),
            _ => panic!("invalid grid_index kind {}", grid_index.kind),
        }
    }
}

impl From<c_GridPlacement> for Line<GridPlacement> {
    fn from(grid_placement: c_GridPlacement) -> Self {
        Self {
            start: GridPlacement::from(grid_placement.start),
            end: GridPlacement::from(grid_placement.end),
        }
    }
}

impl From<c_GridTrackSize> for NonRepeatedTrackSizingFunction {
    fn from(size: c_GridTrackSize) -> Self {
        NonRepeatedTrackSizingFunction {
            min: MinTrackSizingFunction::from(size.min_size),
            max: MaxTrackSizingFunction::from(size.max_size),
        }
    }
}

impl From<c_Length> for MinTrackSizingFunction {
    fn from(length: c_Length) -> Self {
        match length.dim {
            0 => MinTrackSizingFunction::auto(),
            1 => MinTrackSizingFunction::length(length.value),
            2 => MinTrackSizingFunction::percent(length.value),
            3 => MinTrackSizingFunction::min_content(),
            4 => MinTrackSizingFunction::max_content(),
            _ => panic!("unsupported dimension {}", length.dim),
        }
    }
}

impl From<c_Length> for MaxTrackSizingFunction {
    fn from(length: c_Length) -> Self {
        match length.dim {
            0 => MaxTrackSizingFunction::auto(),
            1 => MaxTrackSizingFunction::length(length.value),
            2 => MaxTrackSizingFunction::percent(length.value),
            3 => MaxTrackSizingFunction::min_content(),
            4 => MaxTrackSizingFunction::max_content(),
            5 => MaxTrackSizingFunction::fit_content(LengthPercentage::length(length.value)),
            6 => MaxTrackSizingFunction::fit_content(LengthPercentage::percent(length.value)),
            7 => MaxTrackSizingFunction::fr(length.value),
            _ => panic!("unsupported dimension {}", length.dim),
        }
    }
}

impl From<c_GridTrackSizing> for TrackSizingFunction {
    fn from(value: c_GridTrackSizing) -> Self {
        if value.repetition == -2 {
            if value.single.is_null() {
                panic!("single track sizing is null for single track");
            }
            TrackSizingFunction::Single(NonRepeatedTrackSizingFunction::from(unsafe { *value.single }))
        } else {
            let repeat_slice = if value.repeat_count > 0 && !value.repeat.is_null() {
                unsafe { std::slice::from_raw_parts(value.repeat, value.repeat_count) }
            } else {
                &[]
            };
            TrackSizingFunction::Repeat(
                match value.repetition {
                    -1 => GridTrackRepetition::AutoFit,
                    0 => GridTrackRepetition::AutoFill,
                    n if n > 0 => GridTrackRepetition::Count(n as u16),
                    _ => panic!("invalid repetition {}", value.repetition),
                },
                repeat_slice
                    .iter()
                    .map(|&e| NonRepeatedTrackSizingFunction::from(e))
                    .collect(),
            )
        }
    }
}

impl From<c_Style> for Style {
    fn from(raw: c_Style) -> Self {
        Style {
            display: Display::from_index(raw.display),
            box_sizing: BoxSizing::from_index(raw.box_sizing),
            overflow: taffy::geometry::Point {
                x: Overflow::from_index(raw.overflow_x),
                y: Overflow::from_index(raw.overflow_y),
            },
            scrollbar_width: raw.scrollbar_width,
            position: Position::from_index(raw.position),
            inset: Rect::from(raw.inset),
            align_items: AlignItems::from_index(raw.align_items, raw.has_align_items),
            justify_items: AlignItems::from_index(raw.justify_items, raw.has_justify_items),
            align_self: AlignItems::from_index(raw.align_self, raw.has_align_self),
            justify_self: AlignItems::from_index(raw.justify_self, raw.has_justify_self),
            align_content: AlignContent::from_index(raw.align_content, raw.has_align_content),
            justify_content: AlignContent::from_index(raw.justify_content, raw.has_justify_content),
            gap: Size::from(raw.gap),
            margin: Rect::from(raw.margin),
            border: Rect::from(raw.border),
            padding: Rect::from(raw.padding),
            size: Size::from(raw.size),
            min_size: Size::from(raw.min_size),
            max_size: Size::from(raw.max_size),
            aspect_ratio: if raw.has_aspect_ratio == 0 { None } else { Some(raw.aspect_ratio) },
            flex_wrap: FlexWrap::from_index(raw.flex_wrap),
            flex_direction: FlexDirection::from_index(raw.flex_direction),
            flex_grow: raw.flex_grow,
            flex_shrink: raw.flex_shrink,
            flex_basis: Dimension::from(raw.flex_basis),
            grid_template_rows: if raw.grid_template_rows_count > 0 && !raw.grid_template_rows.is_null() {
                unsafe {
                    std::slice::from_raw_parts(raw.grid_template_rows, raw.grid_template_rows_count)
                }
                .iter()
                .map(|&e| TrackSizingFunction::from(e))
                .collect()
            } else {
                vec![]
            },
            grid_template_columns: if raw.grid_template_columns_count > 0 && !raw.grid_template_columns.is_null() {
                unsafe {
                    std::slice::from_raw_parts(raw.grid_template_columns, raw.grid_template_columns_count)
                }
                .iter()
                .map(|&e| TrackSizingFunction::from(e))
                .collect()
            } else {
                vec![]
            },
            grid_auto_rows: if raw.grid_auto_rows_count > 0 && !raw.grid_auto_rows.is_null() {
                unsafe {
                    std::slice::from_raw_parts(raw.grid_auto_rows, raw.grid_auto_rows_count)
                }
                .iter()
                .map(|&e| NonRepeatedTrackSizingFunction::from(e))
                .collect()
            } else {
                vec![]
            },
            grid_auto_columns: if raw.grid_auto_columns_count > 0 && !raw.grid_auto_columns.is_null() {
                unsafe {
                    std::slice::from_raw_parts(raw.grid_auto_columns, raw.grid_auto_columns_count)
                }
                .iter()
                .map(|&e| NonRepeatedTrackSizingFunction::from(e))
                .collect()
            } else {
                vec![]
            },
            grid_auto_flow: GridAutoFlow::from_index(raw.grid_auto_flow),
            grid_row: Line::from(raw.grid_row),
            grid_column: Line::from(raw.grid_column),
            ..Default::default()
        }
    }
}

// NODES

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_create(tree: *mut TaffyTree, style: *const c_Style) -> u64 {
    let tree = unsafe { &mut *tree };
    let style = unsafe { Style::from(*style) };
    match tree.new_leaf(style) {
        Ok(node) => node.into(),
        Err(_) => 0,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_add_child(tree: *mut TaffyTree, parent: u64, child: u64) -> i32 {
    let tree = unsafe { &mut *tree };
    let parent = NodeId::from(parent);
    let child = NodeId::from(child);
    match tree.add_child(parent, child) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_drop(tree: *mut TaffyTree, node: u64) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    match tree.remove(node) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_drop_all(tree: *mut TaffyTree) {
    let tree = unsafe { &mut *tree };
    tree.clear();
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_replace_child_at_index(tree: *mut TaffyTree, parent: u64, index: usize, child: u64) -> i32 {
    let tree = unsafe { &mut *tree };
    let parent = NodeId::from(parent);
    let child = NodeId::from(child);
    match tree.replace_child_at_index(parent, index, child) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_remove_child(tree: *mut TaffyTree, parent: u64, child: u64) -> i32 {
    let tree = unsafe { &mut *tree };
    let parent = NodeId::from(parent);
    let child = NodeId::from(child);
    match tree.remove_child(parent, child) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_remove_child_at_index(tree: *mut TaffyTree, parent: u64, index: usize) -> i32 {
    let tree = unsafe { &mut *tree };
    let parent = NodeId::from(parent);
    match tree.remove_child_at_index(parent, index) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_dirty(tree: *mut TaffyTree, node: u64) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    match tree.dirty(node) {
        Ok(dirty) => if dirty { 1 } else { 0 },
        Err(_) => -1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_mark_dirty(tree: *mut TaffyTree, node: u64) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    match tree.mark_dirty(node) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_set_style(tree: *mut TaffyTree, node: u64, style: *const c_Style) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    let style = unsafe { Style::from(*style) };
    match tree.set_style(node, style) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_node_set_measure(tree: *mut TaffyTree, node: u64, measure: i32) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    match tree.set_node_context(
        node,
        if measure == 0 {
            None
        } else {
            Some(())
        },
    ) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

// LAYOUT

#[repr(C)]
pub struct c_Layout {
    order: i64,
    location: [f32; 2], // x, y
    size: [f32; 2], // width, height
    content_size: [f32; 2], // width, height
    scrollbar_size: [f32; 2], // width, height
    border: [f32; 4], // top, right, bottom, left
    padding: [f32; 4], // top, right, bottom, left
    margin: [f32; 4], // top, right, bottom, left
}

impl From<Layout> for c_Layout {
    fn from(layout: Layout) -> Self {
        c_Layout {
            order: layout.order as i64,
            location: [layout.location.x, layout.location.y],
            size: [layout.size.width, layout.size.height],
            content_size: [layout.content_size.width, layout.content_size.height],
            scrollbar_size: [layout.scrollbar_size.width, layout.scrollbar_size.height],
            border: [layout.border.top, layout.border.right, layout.border.bottom, layout.border.left],
            padding: [layout.padding.top, layout.padding.right, layout.padding.bottom, layout.padding.left],
            margin: [layout.margin.top, layout.margin.right, layout.margin.bottom, layout.margin.left],
        }
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_compute_layout(tree: *mut TaffyTree, node: u64, available_space: c_Size) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    match tree.compute_layout(node, Size::from(available_space)) {
        Ok(_) => 0,
        Err(_) => 1,
    }
}

#[unsafe(no_mangle)]
pub extern "C" fn taffy_get_layout(tree: *mut TaffyTree, node: u64, layout: *mut c_Layout) -> i32 {
    let tree = unsafe { &mut *tree };
    let node = NodeId::from(node);
    match tree.layout(node) {
        Ok(l) => {
            unsafe {
                *layout = c_Layout::from(*l);
            }
            0
        }
        Err(_) => 1,
    }
}


