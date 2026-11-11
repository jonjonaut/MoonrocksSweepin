<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" MaintainScrollPositionOnPostBack="true" Inherits="Moonrocks_Sweepin.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="stylishsheet.css" />
</head>
<body id="pagin" runat="server">
    <form id="form1" class="form" runat="server">
        <details class="details">
            <summary class="settings">Settings</summary>
            <p class="text">Seed:</p> <asp:TextBox ID="Seed" runat="server"></asp:TextBox>
        </details>
        <div class="labediv">
            <asp:Label ID="flagMode" CssClass="flaglabe" Text="Flag Mode" Visible="False" runat="server"></asp:Label>
        </div>
        <div class="return">
            <asp:Button ID="Return" runat="server" Text="Return to Menu" CssClass="returnBtn" Visible="False" OnClick="ReturnToMenu"/>
        </div>
        <div class="upper">
            <div class="restartClicked">
                <div class="restart">
                    <asp:ImageButton ID="restart" ImageUrl="images/restart.png" CssClass="restartBtn" runat="server" OnClick="Restart"></asp:ImageButton>
                </div>
            </div>
            <div class="moonrocks">
                <asp:Panel ID="Panel" runat="server" CssClass="panel"></asp:Panel>
                <asp:CheckBox ID="flagState" CssClass="flag" AccessKey="F" AutoPostBack="True" runat="server" OnCheckedChanged="ToggleFlag"/>
            </div>
        </div>
    </form>
</body>
</html>
