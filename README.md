# Mimicry
## Telemetry bot for Discord
### Relevant version: 0.0.0 (pre-release)
**Bot for Discord written with C# and Discord.Net 1.0.2**
### How to setup Mimicry?
1. Download `Mimicry.exe`, `cfg` and `libs.zip`
2. Extract `libs.zip` to folder with `Mimicry.exe` and `cfg`
3. Open `cfg`, then write there the token of your App.
4. Save and close `cfg`.
5. Run `Mimicry.exe`. Bot will be runned on your PC.
### Commands
#### Basic commands
- `>echo <MESSAGE>` - bot responds with <MESSAGE>
- `>halt` or `>exit` - bot stops running on PC
- `>getlog` - bot sends logs
- `>getseslog` - bot sends logs from this session
- `>clearlog` - bot clears log file
#### Remote control commands (rc. group)
- `>rc.dir <DIRECTORY FULL PATH>`- bot sends full structure of certain directory
    
**WARNING!** Due to C# formatting, there are some features that must be noticed while writing the directory/file path

**Correct:** `>rc.dir C:\\folder\\1` `>rc.dir "C:\\Folder With Spaces In Its Name"`

**Incorrect:** `>rc.dir C:/1/2/3/4` `>rc.dir C:\12\34`

- `>rc.get <PATH>` - bot sends the file with specified path
- `>rc.load <PATH>, send with attachment` - bot loads the file from the attachment on PC
- `>rc.del <PATH>` - bot deletes the file 
- `>rc.exec <PATH>` - bot executes an .EXE file
- `>rc.getproc` - bot shows launched processes
- `>rc.startup <1/0>` - add/remove bot from automatic startup
- `>rc.kill <ID>` - bot kill a process 
