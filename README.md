# Touchgrass <img width="36" height="36" alt="touching-grass" src="https://github.com/user-attachments/assets/7f981440-83b6-49dd-8e3d-6e29c568edd4" />

A simple CLI Pomodoro timer built in C# .NET. Boost productivity with work/break cycles. Runs on any OS/shell via .NET.

## Installation

1. Clone repo: `git clone https://github.com/yourusername/touchgrass.git`
2. Build: `dotnet build`
3. Run: `dotnet run --project Touchgrass` (alias as `tgrass` for convenience)

## Usage

`tgrass` (default: 4 cycles, 25m work, 5m break)

- Customize: `tgrass --work 30 --break 10 --cycles 3`
- Testing mode: `tgrass --testing 10` (1 cycle, 10s work/break)

Interactive: Confirms phases, restarts after cycles.

## Options

- `--work <min>`: Work duration
- `--break <min>`: Break duration
- `--cycles <num>`: Cycle count
- `--testing <sec>`: Quick test mode
