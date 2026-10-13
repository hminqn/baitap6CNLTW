param([int]$Id=6, [string]$BaseUrl='http://localhost:5186')
$ErrorActionPreference='Stop'
$session=New-Object Microsoft.PowerShell.Commands.WebRequestSession
$details=Invoke-WebRequest "$BaseUrl/Books/Details/$Id" -WebSession $session -UseBasicParsing
if (!$details.Content.Contains('CRUD-SMOKE-') -or !$details.Content.Contains('-EDITED')) { throw 'Saved test record not found after restart' }
Write-Output 'PASS: Updated book persists after application restart'
$html=(Invoke-WebRequest "$BaseUrl/Books/Delete/$Id" -WebSession $session -UseBasicParsing).Content
$token=[regex]::Match($html,'name="__RequestVerificationToken" type="hidden" value="([^"]+)"').Groups[1].Value
if (!$token) { throw 'Missing anti-forgery token' }
Invoke-WebRequest "$BaseUrl/Books/Delete/$Id" -WebSession $session -UseBasicParsing -Method Post -Body @{__RequestVerificationToken=[System.Net.WebUtility]::HtmlDecode($token)} | Out-Null
$missing=$false
try { Invoke-WebRequest "$BaseUrl/Books/Details/$Id" -UseBasicParsing | Out-Null } catch { $missing=[int]$_.Exception.Response.StatusCode -eq 404 }
if (!$missing) { throw 'Delete failed' }
Write-Output 'PASS: Delete removes saved record (404 verified)'

