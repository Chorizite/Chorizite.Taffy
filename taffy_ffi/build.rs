fn main() {
    csbindgen::Builder::default()
        .input_extern_file("src/lib.rs")
        .csharp_dll_name("taffy_ffi")
        .csharp_namespace("TaffySharp.Lib")
        .csharp_class_name("NativeMethods")
        .csharp_class_accessibility("internal") 
        .generate_csharp_file("../TaffySharp/Lib/NativeMethods.g.cs")
        .unwrap();
}