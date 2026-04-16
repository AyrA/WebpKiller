# WebpKiller

Application that monitors given folders and silently converts all webp files into jpeg,
then deletes the webp files.

## Building

Should build as-is, not a single external dependency, just .NET 10

## Usage

Simply run the command and supply as many folders to watch as you want.
The application also watches all subfolders.

    WebpKiller.exe <path> [path ...]

It will retry the conversion for up to 10 seconds
to accomodate utilities that keep the webp file handle open for a while
