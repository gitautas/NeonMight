.\fabric\win\env.ps1

echo "Killing Neon White process if it is running..."
Stop-Process -Name "Neon White" -Force 2>$null
Start-Sleep -Seconds 1 # Stop-Process finishes before the lock on NeonMight.dll is released so Copy-Item fails :/

echo "Moving build output to Mods directory..."
Copy-Item ".\bin\Debug\net472\NeonMight.dll" -Destination "$env:MODS_PATH\NeonMight.dll"

echo "Starting Neon White..."
explorer "steam://rungameid/1533420"