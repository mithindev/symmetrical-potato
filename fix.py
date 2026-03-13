import re

file_path = r'c:\Users\mithi\Desktop\Fiscus\DepositReceipt.aspx.vb'
with open(file_path, 'r', encoding='utf-8-sig') as f:
    content = f.read()

def replacer(match):
    indent = match.group(1)
    ex_var = match.group(2)
    return (
        f"{indent}Dim msg = {ex_var}.ToString().Replace(\"'\", \"\").Replace(vbCrLf, \" \")\n"
        f"{indent}Dim fnc As String = \"showToastnOK('Error', '\" + msg + \"');\"\n"
        f"{indent}ScriptManager.RegisterStartupScript(Me, Me.GetType(), \"showtoast\", fnc, True)"
    )

new_content = re.sub(r'([ \t]+)Response\.Write\((.*?)\)', replacer, content)

if new_content != content:
    with open(file_path, 'w', encoding='utf-8-sig') as f:
        f.write(new_content)
    print("Replaced occurrences")
else:
    print("No occurrences found")
