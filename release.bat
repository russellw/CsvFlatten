set version=1.0

"\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild.exe" CsvFlatten.sln /p:Configuration=Release /p:Platform="Any CPU"
if errorlevel 1 goto :eof

md CsvFlatten-%version%
copy App.config CsvFlatten-%version%
copy LICENSE CsvFlatten-%version%
copy README.md CsvFlatten-%version%
copy bin\Release\CsvFlatten.exe CsvFlatten-%version%

del CsvFlatten-%version%.zip
7z a CsvFlatten-%version%.zip CsvFlatten-%version%
7z l CsvFlatten-%version%.zip

rd /q /s CsvFlatten-%version%
