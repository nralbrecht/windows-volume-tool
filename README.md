# Windows Volume Tool

![test](/images/usage.png)

This is a small command line tool to change the volume of individual programs. It uses the Windows Core Audio APIs to change the volume. You can see it as automating the Windows Volume Mixer.

## Example
List available audio sessions:
```
$ windows-volume-tool.exe list
Spotify
Discord
firefox
```

Decrease volume of Firefox by 10%:
```
$ windows-volume-tool.exe add -0.1 firefox
```

Mute Firefox:
```
$ windows-volume-tool.exe mute firefox
```

Toggle the mute state of Firefox back to on:
```
$ windows-volume-tool.exe toggle firefox
```

Set volume of Firefox to 50%:
```
$ windows-volume-tool.exe set 0.5 firefox
```

## Usage
```
Usage: windows-volume-tool.exe <command> [options]

  windows-volume-tool.exe list                            List available audio sessions
  windows-volume-tool.exe help                            Print this help page
  windows-volume-tool.exe set <percentage> <name>         Set volume of the session to the given percentage
  windows-volume-tool.exe add [+|-]<percentage> <name>    Add the given percentage to the volume of the session
  windows-volume-tool.exe mute <name>                     Mute the session
  windows-volume-tool.exe unmute <name>                   Unmute the session
  windows-volume-tool.exe toggle <name>                   Mute/unmute the session

  Percentage values have to be given as a number between 0.0 (0%) and 1.0 (100%).
```