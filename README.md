# WebpKiller

Application that monitors given folders and silently converts all webp files into jpeg,
then deletes the webp files.

## Building

Needs .NET 10. Should build as-is. Open a terminal in the directory that contains the csproj file,
then execute `dotnet build -c release`

The exe will be in the `bin\Release` folder

## Usage

Before using, install imagemagick: https://imagemagick.org/download/#windows

Simply run the command and supply as many folders to watch as you want.
The application also watches all subfolders.

    WebpKiller.exe <path> [path ...]

*Tip: Add to your startup folder to launch it when you start your computer*

It will retry the conversion for up to 10 seconds
to accomodate utilities that keep the webp file handle open for a while

Note: The application has no UI and no official way to exit because I did not intend to publish it.
You can kill it via Task Manager if you want to exit it.
