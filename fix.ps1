$filePath = "c:\Users\mithi\Desktop\Fiscus\DepositReceipt.aspx.vb"
$content = Get-Content -Path $filePath -Raw
$pattern = "(?m)^([ \t]+)Response\.Write\((.*)\)"
$replacement = "`$1Dim msg = `$2.ToString().Replace(`"'`", `"`").Replace(vbCrLf, `' `')`n`$1Dim fnc As String = `"showToastnOK('Error', '`" + msg + `"');`"`n`$1ScriptManager.RegisterStartupScript(Me, Me.GetType(), `"showtoast`", fnc, True)"
$newContent = [regex]::Replace($content, $pattern, $replacement)

[System.IO.File]::WriteAllText($filePath, $newContent, [System.Text.Encoding]::UTF8)
Write-Output "Replaced occurrences"
