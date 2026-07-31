# Touchgrass <img width="36" height="36" alt="touching-grass" src="https://github.com/user-attachments/assets/7f981440-83b6-49dd-8e3d-6e29c568edd4" />

A simple CLI Pomodoro timer built in C# .NET. Boost productivity with work/break cycles. Runs on any OS/shell via .NET.

## Usage

**Default: 4 cycles, 25m work, 5m break**

- Customize: `dotnet run --project ./src/Touchgrass --work 30 --break 10 --cycles 3`
- Testing mode: `dotnet run --project ./src/Touchgrass --testing 10` (1 cycle, 10s work/break)

Interactive: Confirms phases, restarts after cycles.

## Options

- `--work <min>`: Work duration
- `--break <min>`: Break duration
- `--cycles <num>`: Cycle count
- `--testing <sec>`: Quick test mode
