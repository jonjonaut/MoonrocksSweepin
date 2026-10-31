<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="Moonrocks_Sweepin.Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Moonrocks</title>
    <link rel="stylesheet" href="stylishsheet.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="Height" runat="server"></asp:TextBox>
            <asp:TextBox ID="Width" runat="server"></asp:TextBox>
            <asp:Button ID="submit" runat="server" Text="submit" OnClick="submit_Click" />
        </div>
    </form>
</body>
</html>
