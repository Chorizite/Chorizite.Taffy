

# build x64
(cd taffy_ffi && cargo build --target i686-pc-windows-msvc --release)
cp taffy_ffi/target/i686-pc-windows-msvc/release/taffy_ffi.dll TaffySharp/runtimes/win-x86/native/
echo "Copied x86 dll to TaffySharp/runtimes/win-x86/native/"

# build x86
(cd taffy_ffi && cargo build --target x86_64-pc-windows-msvc --release)
cp taffy_ffi/target/x86_64-pc-windows-msvc/release/taffy_ffi.dll TaffySharp/runtimes/win-x64/native/
echo "Copied x64 dll to TaffySharp/runtimes/win-x64/native/"