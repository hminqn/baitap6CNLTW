param([string]$BaseUrl = "http://localhost:5186", [switch]$KeepRecord)
$ErrorActionPreference = 'Stop'
$session = New-Object Microsoft.PowerShell.Commands.WebRequestSession
function Get-Page([string]$Path) { Invoke-WebRequest "$BaseUrl$Path" -WebSession $session -UseBasicParsing }
function Get-Token([string]$Path) {
    $html = (Get-Page $Path).Content
    $match = [regex]::Match($html, 'name="__RequestVerificationToken" type="hidden" value="([^"]+)"')
    if (!$match.Success) { throw "Missing anti-forgery token on $Path" }
    [System.Net.WebUtility]::HtmlDecode($match.Groups[1].Value)
}
function Assert([bool]$Condition, [string]$Message) { if (!$Condition) { throw $Message }; Write-Output "PASS: $Message" }
$title = 'CRUD-SMOKE-' + [guid]::NewGuid().ToString('N')
Assert ((Get-Page '/').StatusCode -eq 200) 'List responds successfully'
$token = Get-Token '/Books/Create'
$invalid = Invoke-WebRequest "$BaseUrl/Books/Create" -WebSession $session -UseBasicParsing -Method Post -Body @{__RequestVerificationToken=$token; Title=''; Author=''; Category=''; PublishedYear=0; Price=-1; Quantity=-1}
Assert ($invalid.Content.Contains('field-validation-error')) 'Invalid data is rejected'
$blocked = $false
try { Invoke-WebRequest "$BaseUrl/Books/Create" -WebSession $session -UseBasicParsing -Method Post -Body @{Title=$title} | Out-Null }
catch { $blocked = [int]$_.Exception.Response.StatusCode -eq 400 }
Assert $blocked 'POST without anti-forgery token is rejected'
$body = @{ __RequestVerificationToken=(Get-Token '/Books/Create'); Title=$title; Author='Smoke Author'; Category='Smoke Category'; Publisher='Test'; PublishedYear=2024; Price='75000'; Quantity=3; Description='Persistence test' }
Invoke-WebRequest "$BaseUrl/Books/Create" -WebSession $session -UseBasicParsing -Method Post -Body $body | Out-Null
$list = (Get-Page ('/Books?search=' + $title)).Content
$match = [regex]::Match($list, '/Books/Details/(\d+)')
Assert $match.Success 'Created book is searchable'
$id = $match.Groups[1].Value
$details = (Get-Page "/Books/Details/$id").Content
Assert ($details.Contains($title) -and $details.Contains('Smoke Author')) 'Details displays saved data'
$body.__RequestVerificationToken = Get-Token "/Books/Edit/$id"
$body.Id = $id
$body.Title = $title + '-EDITED'
$body.Quantity = 7
Invoke-WebRequest "$BaseUrl/Books/Edit/$id" -WebSession $session -UseBasicParsing -Method Post -Body $body | Out-Null
Assert ((Get-Page "/Books/Details/$id").Content.Contains($title+'-EDITED')) 'Edit is saved'
Assert ((Get-Page ('/Books?search='+$title+'&category=Smoke%20Category&page=999')).Content.Contains($title+'-EDITED')) 'Combined filter and page bounds work'
if ($KeepRecord) { Write-Output "KEPT_RECORD_ID=$id"; exit 0 }
$delete = @{ __RequestVerificationToken=(Get-Token "/Books/Delete/$id") }
Invoke-WebRequest "$BaseUrl/Books/Delete/$id" -WebSession $session -UseBasicParsing -Method Post -Body $delete | Out-Null
$missing=$false
try { Get-Page "/Books/Details/$id" | Out-Null } catch { $missing=[int]$_.Exception.Response.StatusCode -eq 404 }
Assert $missing 'Delete removes record from database'
Write-Output 'All CRUD smoke checks passed.'

