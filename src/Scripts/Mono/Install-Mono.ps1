function IsAdmin
{
	$wid=[System.Security.Principal.WindowsIdentity]::GetCurrent()	
	$prp=new-object System.Security.Principal.WindowsPrincipal($wid)
	$adm=[System.Security.Principal.WindowsBuiltInRole]::Administrator
	$IsAdmin=$prp.IsInRole($adm)
	return($IsAdmin)
}

function Is64Bit
{
	return (gwmi Win32_OperatingSystem).OSArchitecture -eq '64-bit'
}

# http://poshcode.org/417
## Get-WebFile (aka wget for PowerShell)
##############################################################################################################
## Downloads a file or page from the web
## History:
## v3.6 - Add -Passthru switch to output TEXT files
## v3.5 - Add -Quiet switch to turn off the progress reports ...
## v3.4 - Add progress report for files which don't report size
## v3.3 - Add progress report for files which report their size
## v3.2 - Use the pure Stream object because StreamWriter is based on TextWriter:
##        it was messing up binary files, and making mistakes with extended characters in text
## v3.1 - Unwrap the filename when it has quotes around it
## v3   - rewritten completely using HttpWebRequest + HttpWebResponse to figure out the file name, if possible
## v2   - adds a ton of parsing to make the output pretty
##        added measuring the scripts involved in the command, (uses Tokenizer)
##############################################################################################################
function Get-WebFile {
param(
  $url = '', #(Read-Host "The URL to download"),
  $fileName = $null,
  [switch]$Passthru,
  [switch]$quiet
)
  Write-Debug "Running 'Get-WebFile' for $fileName with url:`'$url`'";
  #if ($url -eq '' return)
  $req = [System.Net.HttpWebRequest]::Create($url);
  #to check if a proxy is required
  $webclient = new-object System.Net.WebClient
  if (!$webclient.Proxy.IsBypassed($url))
  {
	$creds = [net.CredentialCache]::DefaultCredentials
	if ($creds -eq $null) {
	  Write-Debug "Default credentials were null. Attempting backup method"
	  $cred = get-credential
	  $creds = $cred.GetNetworkCredential();
	}
	$proxyaddress = $webclient.Proxy.GetProxy($url).Authority
	Write-host "Using this proxyserver: $proxyaddress"
	$proxy = New-Object System.Net.WebProxy($proxyaddress)
	$proxy.credentials = $creds
	$req.proxy = $proxy
  }
 
  #http://stackoverflow.com/questions/518181/too-many-automatic-redirections-were-attempted-error-message-when-using-a-httpw
  $req.CookieContainer = New-Object System.Net.CookieContainer
  $res = $req.GetResponse();

  if($fileName -and !(Split-Path $fileName)) {
	$fileName = Join-Path (Get-Location -PSProvider "FileSystem") $fileName
  }
  elseif((!$Passthru -and ($fileName -eq $null)) -or (($fileName -ne $null) -and (Test-Path -PathType "Container" $fileName)))
  {
	[string]$fileName = ([regex]'(?i)filename=(.*)$').Match( $res.Headers["Content-Disposition"] ).Groups[1].Value
	$fileName = $fileName.trim("\/""'")
	if(!$fileName) {
	   $fileName = $res.ResponseUri.Segments[-1]
	   $fileName = $fileName.trim("\/")
	   if(!$fileName) {
		  $fileName = Read-Host "Please provide a file name"
	   }
	   $fileName = $fileName.trim("\/")
	   if(!([IO.FileInfo]$fileName).Extension) {
		  $fileName = $fileName + "." + $res.ContentType.Split(";")[0].Split("/")[1]
	   }
	}
	$fileName = Join-Path (Get-Location -PSProvider "FileSystem") $fileName
  }
  if($Passthru) {
	$encoding = [System.Text.Encoding]::GetEncoding( $res.CharacterSet )
	[string]$output = ""
  }

  if($res.StatusCode -eq 200) {
	[long]$goal = $res.ContentLength
	$reader = $res.GetResponseStream()
	if($fileName) {
	   $writer = new-object System.IO.FileStream $fileName, "Create"
	}
	[byte[]]$buffer = new-object byte[] 1048576
	[long]$total = [long]$count = [long]$iterLoop =0
	do
	{
	   $count = $reader.Read($buffer, 0, $buffer.Length);
	   if($fileName) {
		  $writer.Write($buffer, 0, $count);
	   }
	   if($Passthru){
		  $output += $encoding.GetString($buffer,0,$count)
	   } elseif(!$quiet) {
		  $total += $count
		  if($goal -gt 0 -and ++$iterLoop%10 -eq 0) {
			 Write-Progress "Downloading $url to $fileName" "Saving $total of $goal" -id 0 -percentComplete (($total/$goal)*100) 
		  }
		  if ($total -eq $goal) {
			Write-Progress "Completed download of $url." "Completed a total of $total bytes of $fileName" -id 0 -Completed 
		  }
	   }
	} while ($count -gt 0)
   
	$reader.Close()
	if($fileName) {
	   $writer.Flush()
	   $writer.Close()
	}
	if($Passthru){
	   $output
	}
  }
  $res.Close();
}

function Start-ProcessAsAdmin {
param(
  [string] $statements, 
  [string] $exeToRun = 'powershell'
)
  $validExitCodes = @(0)  
@"
Elevating Permissions and running $exeToRun $statements. This may take awhile, depending on the statements.
"@ | Write-Host
  $psi = new-object System.Diagnostics.ProcessStartInfo;
  $psi.FileName = $exeToRun;
  if ($statements -ne '') {
	$psi.Arguments = "$statements";
  }
  if ([Environment]::OSVersion.Version -ge (new-object 'Version' 6,0)){
	$psi.Verb = "runas";
  }
  $psi.WorkingDirectory = get-location;
  if ($minimized) {
	$psi.WindowStyle = [System.Diagnostics.ProcessWindowStyle]::Minimized;
  } 
  $s = [System.Diagnostics.Process]::Start($psi);
  $s.WaitForExit();
  if ($validExitCodes -notcontains $s.ExitCode) {
	$errorMessage = "[ERROR] Running $exeToRun with $statements was not successful. Exit code was `'$($s.ExitCode)`'."
	Write-Error $errorMessage
	throw $errorMessage
  }
}

function Install-ExeFileFromUrl {
param(
  [string] $silentArgs = '',
  [string] $url,
  [string] $packageName
)
  try {
	$tempDir = Join-Path $env:TEMP "PS1Install"
	$tempDir = Join-Path $tempDir "$packageName"
	if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir) | Out-Null}
	$file = Join-Path $tempDir "$($packageName).exe"

	Write-Host "Downloading file from url:`'$url`', path:`'$file`'";
	Get-WebFile $url $file  
	Start-Sleep 2 #give it a sec or two to finish up
	Write-Host "Installing package from url:`'$url`', args: `'$silentArgs`' ";
	Start-ProcessAsAdmin $silentArgs $file 
  } catch {
	Write-Host "ERROR: Installing package $packageName failed with: $($_.Exception.Message)!" -ForegroundColor red	
	throw
  }
}

function Install-VsixFromUrl {
param(
  [string] $url
)
  try {
	$tempDir = Join-Path $env:TEMP "PS1Install\VSIX"
	if (![System.IO.Directory]::Exists($tempDir)) {[System.IO.Directory]::CreateDirectory($tempDir) | Out-Null}
	$file = Join-Path $tempDir "temp.vsix"

	Write-Host "Downloading file from url:`'$url`', path:`'$file`'";
	Get-WebFile $url $file  
	Start-Sleep 2 #give it a sec or two to finish up
	Write-Host "Installing package from url:`'$url`'";

	$exe = Join-Path $([Environment]::GetEnvironmentVariable("ProgramFiles(x86)")) "Common7"
	$exe = Join-Path $exe "IDE"
	$exe = Join-Path $exe "VSIXInstaller.exe"

	Start-ProcessAsAdmin $file $exe
  } catch {
	Write-Host "ERROR: Installing package $packageName failed with: $($_.Exception.Message)!" -ForegroundColor red	
	throw
  }
}


$isadmin = IsAdmin
if(!$isadmin)
{
	Write-Host "ERROR: Running as administrator is required for this script!" -ForegroundColor red
	Read-Host "Press ENTER to quit"
	exit;
}

# am I running in 32 bit shell?
if ($pshome -like "*syswow64*") {	
	write-warning "Restarting script under 64 bit powershell"

	# relaunch this script under 64 bit shell
	& (join-path ($pshome -replace "syswow64", "sysnative") powershell.exe) -file `
		(join-path (Get-Location) $myinvocation.mycommand) @args

	# exit 32 bit script
	exit
}

$packageName = 'mono3' # arbitrary name for the package, used in messages
$url = 'http://download.mono-project.com/archive/3.2.3/windows-installer/mono-3.2.3-gtksharp-2.12.11-win32-0.exe'
$silentArgs = '/SILENT' # "/s /S /q /Q /quiet /silent /SILENT /VERYSILENT"

try{
	#Install-ExeFileFromUrl $silentArgs $url $packageName
}
catch{
	#just swallow and move next
}

$packageName = 'mono2' # arbitrary name for the package, used in messages
$url = 'http://download.mono-project.com/archive/2.10.9/windows-installer/0/mono-2.10.9-gtksharp-2.12.11-win32-0.exe'
$silentArgs = '/SILENT' # "/s /S /q /Q /quiet /silent /SILENT /VERYSILENT"

try{
	#Install-ExeFileFromUrl $silentArgs $url $packageName
}
catch{
	#just swallow and move next
}

$redist = @"
<?xml version="1.0" encoding="utf-8"?>
<FileList Redist="Mono-3.2.3" Name="Mono 3.2.3 Profile (4.5)" RuntimeVersion="4.5" ToolsVersion="4.0" ShortName="Mono">
</FileList>
"@

if (Is64Bit) { 
	#32
	$programFilesPath = "C:\Program Files (x86)"
} 
else { 
	#64
	$programFilesPath = "C:\Program Files"
}



Copy-Item "$programFilesPath\Mono-3.2.3\lib\mono\4.5\*" "$programFilesPath\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Profile\Mono" -recurse
New-Item "$programFilesPath\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Profile\Mono\RedistList\FrameworkList.xml" -type file -force -value $redist

$redist = @"
<?xml version="1.0" encoding="utf-8"?>
<FileList Redist="Mono-3.2.3" Name="Mono 3.2.3 Profile (4)" RuntimeVersion="4.0" ToolsVersion="4.0" ShortName="Mono4">
</FileList>
"@

Copy-Item "$programFilesPath\Mono-3.2.3\lib\mono\4.0\*" "$programFilesPath\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Mono" -recurse
New-Item "$programFilesPath\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Profile\Mono\RedistList\FrameworkList.xml" -type file -force -value $redist

Write-Host "Installing registry keys"
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.5,Profile=Mono" /f
REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0,Profile=Mono" /f
if (Is64Bit) { 
	REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.5,Profile=Mono" /f
	REG ADD "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\.NETFramework\v4.0.30319\SKUs\.NETFramework,Version=v4.0,Profile=Mono" /f
}

Install-VsixFromUrl 'http://visualstudiogallery.msdn.microsoft.com/cb83d210-b09f-4e21-949e-81ad23684c78/file/89845/7/MonoHelper.vsix'